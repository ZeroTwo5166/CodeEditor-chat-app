using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.tests
{
    public static class MockDbSet<T> where T : class
    {
        public static DbSet<T> Create(params T[] elements)
        {
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elements.AsQueryable().Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elements.AsQueryable().Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elements.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elements.AsQueryable().GetEnumerator());

            return mockDbSet.Object;
        }
    }
}
