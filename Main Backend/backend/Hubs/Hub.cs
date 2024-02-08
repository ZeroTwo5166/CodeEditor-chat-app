using backend.ModelsDTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;

namespace backend.Hubs
{
    public class Hub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IDictionary<string, UserRoomConnectionDTO> _connection; //Context.ConnectionId
        private readonly IConfiguration _configuration;
        private readonly IDictionary<string, ProjectDTO> _projectList; // key = room, value = ProjectDto

        public Hub(IDictionary<string, UserRoomConnectionDTO> connection,
            IConfiguration configuration,
            IDictionary<string, ProjectDTO> projectList
            )
        {
            _connection = connection;
            _configuration = configuration;
            _projectList = projectList;

        }

        public async Task JoinRoom(UserRoomConnectionDTO user, ProjectDTO project)
        {
            //If project title is provided, add the room as key and projectTitle as value in _projectList Dict (create project with room)
            if (!string.IsNullOrEmpty(project.ProjectName))
            {
                _projectList[user.Room] = project;
            }
            else
            {
                // Check if the user does not provide a projectTitle and joins a room which is not in the _projectListDict
                if (!_projectList.ContainsKey(user.Room))
                {
                    // Send a message to only the user indicating that the room does not exist
                    await Clients.Client(Context.ConnectionId).SendAsync("RoomErrorMessage", "Room not found!");
                    return; // Exit the method if the room doesn't exist
                }
                else
                {
                    // User joined without providing projectTitle, search for the corresponding ProjectDto in _projectList
                    ProjectDTO existingProject = _projectList[user.Room];

                    // Set the projectTitle and projectId from the existing ProjectDto to the parameter project
                    project.ProjectName = existingProject.ProjectName;
                    project.ProjectId = existingProject.ProjectId;
                }
            }




        }

        // Add a method to broadcast document updates to clients
        public async Task BroadcastDocumentUpdate(string updatedDocument)
        {
            //Check is the caller is in the dictionary (i.e. if he is connected or not)
            if (_connection.TryGetValue(Context.ConnectionId, out UserRoomConnectionDTO userRoomConnection))
            {
                if (!string.IsNullOrEmpty(updatedDocument))
                {
                    // Obtain projectId from _projectList using user's room
                    if (_projectList.TryGetValue(userRoomConnection.Room, out ProjectDTO project))
                    {
                        Guid projectId = (Guid)project.ProjectId;

                        // Get all the users with the same room (which also means the project that they are working on)
                        List<string> connectionIds = _connection.Keys
                            .Where(connectionId => _connection[connectionId].Room == userRoomConnection.Room)
                            .ToList();

                        // Update Code for each user in the group
                        foreach (var connectionId in connectionIds)
                        {
                            if (_connection.TryGetValue(connectionId, out UserRoomConnectionDTO connection))
                            {
                                connection.Code = updatedDocument;
                            }
                        }

                        // Send message to all users in the group/room
                        await Clients.Group(userRoomConnection.Room!)
                            .SendAsync("ReceiveDocumentUpdate", updatedDocument);

                        // Update the database with the latest document
                        await UpdateDatabase(projectId, updatedDocument);
                    }
                }

            }
            else
            {
                // Handle the case where the project is not found in the _projectList
                await Clients.Client(Context.ConnectionId).SendAsync("RoomErrorMessage", "Project not found!");
            }
        }

        //Updating database 
        private async Task UpdateDatabase(Guid projectId, string updatedDocument)
        {
            try
            {
                if (updatedDocument != "")
                {
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();

                        //Check if there is a project with the projectId provided in the parameter
                        string checkQuery = "Select COUNT(*) FROM Projects WHERE Id = @Id";
                        using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@Id", projectId);
                            int count = (int)await checkCommand.ExecuteScalarAsync();

                            //If project exists
                            if (count > 0)
                            {
                                // Title exists, perform update
                                string updateQuery = "UPDATE Projects SET Code = @Code WHERE Id = @Id";
                                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@Id", projectId);
                                    command.Parameters.AddWithValue("@Code", updatedDocument);
                                    await command.ExecuteNonQueryAsync();
                                }
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle database update error
                Console.WriteLine($"Error updating database: {ex.Message}");
            }
        }

        public async Task<string> GetInitialData()
        {
            if (_connection.TryGetValue(Context.ConnectionId, out UserRoomConnectionDTO userRoomConnection))
            {
                return userRoomConnection.Code;
            }

            return null;
        }

        public async Task<string> GetCodeFromRoom(string room)
        {

            // Iterate through the values to find the entry with the matching room
            var userRoomConnection = _connection.Values.FirstOrDefault(connection => connection.Room == room);

            if (userRoomConnection != null)
            {
                return userRoomConnection.Code;
            }

            // Return a default value or handle the case where the room is not found
            return null;
        }

        public override Task OnDisconnectedAsync(Exception? exp)
        {
            if (!_connection.TryGetValue(Context.ConnectionId, out UserRoomConnectionDTO roomConnection))
            {
                return base.OnDisconnectedAsync(exp);
            }

            // _connection.Remove(Context.ConnectionId);
            Clients.Group(roomConnection.Room!)
                .SendAsync("LeaveMessage", "Lets Program bot", $"{roomConnection.User} has Left the Group", DateTime.Now);


            // Call the function to check and remove the room if no users are connected
            RemoveRoomIfNoUsers(roomConnection.Room);

            return base.OnDisconnectedAsync(exp);
        }

        public IDictionary<string, UserRoomConnectionDTO> GetConnectionDictionary()
        {
            return _connection;
        }

        public IDictionary<string, ProjectDTO> GetProjectDictionary()
        {
            return _projectList;
        }

        public bool ContainsRoom(string room)
        {
            return _projectList.Any(entry => entry.Key == room);
        }

        public void RemoveRoomIfNoUsers(string room)
        {
            // Check if there are any connected users in the specified room
            bool hasConnectedUsers = _connection.Values.Any(connection => connection.Room == room);

            if (!hasConnectedUsers)
            {
                // No connected users in the room, remove it from the project list
                if (_projectList.ContainsKey(room))
                {
                    _projectList.Remove(room);
                }
            }
        }
        public void DisconnectUserByUsername(string username)
        {
            var keyValuePair = _connection.FirstOrDefault(connection => connection.Value.User == username);
            if (!Equals(keyValuePair, default(KeyValuePair<string, UserRoomConnectionDTO>)))
            {
                // Remove the key-value pair from the dictionary
                _connection.Remove(keyValuePair.Key);

                // Call the function to check and remove the room if no users are connected
                RemoveRoomIfNoUsers(keyValuePair.Value.Room);

                // Notify clients that the user has left the group
                Clients.Group(keyValuePair.Value.Room)
                    .SendAsync("LeaveMessage", "Lets Program bot", $"{keyValuePair.Value.User} has Left the Group", DateTime.Now);
            }



        }


    }



}
