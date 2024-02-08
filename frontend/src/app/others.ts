
export const languageOptionsForMonacoEditor : string[] = [
    "plaintext", "abap", "apex", "azcli", "bat", "bicep", "cameligo", "clojure", "coffeescript",
    "c", "cpp", "csharp", "csp", "css", "cypher", "dart", "dockerfile", "ecl", "elixir", "flow9",
    "fsharp", "freemarker2", "go", "graphql", "handlebars", "hcl", "html",
    "ini", "java", "javascript", "julia", "kotlin", "less", "lexon", "lua", "liquid", "m3",
    "markdown", "mdx", "mips", "msdax", "mysql", "objective-c", "pascal", "pascaligo", "perl",
    "pgsql", "php", "pla", "postiats", "powerquery", "powershell", "proto", "pug", "python",
    "qsharp", "r", "razor", "redis", "redshift", "restructuredtext", "ruby", "rust", "sb", "scala",
    "scheme", "scss", "shell", "sol", "aes", "sparql", "sql", "st", "swift", "systemverilog", "verilog",
    "tcl", "twig", "typescript", "vb", "wgsl", "xml", "yaml", "json"
];

export const ace_themes = {
    'Chrome': 'chrome',
    'Clouds': 'clouds',
    'Crimson Editor': 'crimson_editor',
    'Dawn': 'dawn',
    'Dreamweaver': 'dreamweaver',
    'Eclipse': 'eclipse',
    'GitHub': 'github',
    'IPlastic': 'iplastic',
    'KatzenMilch': 'katzenmilch',
    'Kuroir': 'kuroir',
    'Solarized Light': 'solarized_light',
    'SQL Server': 'sqlserver',
    'TextMate': 'textmate',
    'Tomorrow': 'tomorrow',
    'XCode': 'xcode',
    'Ambiance': 'ambiance',
    'Chaos': 'chaos',
    'Clouds Midnight': 'clouds_midnight',
    'Cobalt': 'cobalt',
    'Dracula': 'dracula',
    'Greeon on Black': 'gob',
    'Gruvbox': 'gruvbox',
    'idle Fingers': 'idle_fingers',
    'krTheme': 'kr_theme',
    'Merbivore': 'merbivore',
    'Merbivore Soft': 'merbivore_soft',
    'Mono Industrial': 'mono_industrial',
    'Monokai': 'monokai',
    'Pastel on Dark': 'pastel_on_dark',
    'Solarized Dark': 'solarized_dark',
    'Terminal': 'terminal',
    'Tomorrow Night': 'tomorrow_night',
    'Tomorrow Night Blue': 'tomorrow_night_blue',
    'Tomorrow Night Bright': 'tomorrow_night_bright',
    'Tomorrow Night 80s': 'tomorrow_night_eighties',
    'Twilight': 'twilight',
    'Vibrant Ink': 'vibrant_ink'
};

export const allAceLanguages = {
    'Actionscript': 'actionscript',
    'Apache_conf': 'apache_conf',
    'Apex': 'apex',
    'Applescript': 'applescript',
    'Asciidoc': 'asciidoc',
    'Asl': 'asl',
    'Assembly_x86': 'assembly_x86',
    'Astro': 'astro',
    'Batchfile': 'batchfile',
    'Clojure': 'clojure',
    'Cobol': 'cobol',
    'Coffee': 'coffee',
    'Crystal': 'crystal',
    'Csharp': 'csharp',
    'Css': 'css',
    'Django': 'django',
    'Dart': 'dart',
    'Dockerfile': 'dockerfile',
    'Dot': 'dot',
    'Elixir': 'elixir',
    'Erlang': 'erlang',
    'Fsharp': 'fsharp',
    'Gitignore': 'gitignore',
    'Golang': 'golang',
    'Groovy': 'groovy',
    'Haml': 'haml',
    'Hjson': 'hjson',
    'Html': 'html',
    'Io': 'io',
    'Jack': 'jack',
    'Java': 'java',
    'Javascript': 'javascript',
    'Json': 'json',
    'Jsx': 'jsx',
    'Julia': 'julia',
    'Kotlin': 'kotlin',
    'Latex': 'latex',
    'Livescript': 'livescript',
    'Lua': 'lua',
    'Matlab': 'matlab',
    'Mushcode': 'mushcode',
    'Mysql': 'mysql',
    'Nasal': 'nasal',
    'Nginx': 'nginx',
    'Nim': 'nim',
    'Objectivec': 'objectivec',
    'Odin': 'odin',
    'Pascal': 'pascal',
    'Perl': 'perl',
    'Php': 'php',
    'Plain_text': 'plain_text',
    'Powershell': 'powershell',
    'Puppet': 'puppet',
    'Python': 'python',
    'R': 'r',
    'Razor': 'razor',
    'Robot': 'robot',
    'Ruby': 'ruby',
    'Rust': 'rust',
    'Sass': 'sass',
    'Scss': 'scss',
    'Slim': 'slim',
    'Snippets': 'snippets',
    'Sql': 'sql',
    'Sqlserver': 'sqlserver',
    'Svg': 'svg',
    'Swift': 'swift',
    'Text': 'text',
    'Tsx': 'tsx',
    'Turtle': 'turtle',
    'Typescript': 'typescript',
    'Velocity': 'velocity',
    'Xml': 'xml',
    'Xquery': 'xquery',
    'Yaml': 'yaml',
    'Zeek': 'zeek'
}
  
export const popularAceLaguage = {
    'Javascript': 'javacript',
    'Python': 'python',
    'Java': 'java',
    'Ruby': 'ruby',
    'C#': 'csharp',
    'PHP': 'php',
    'TypeScript': 'typescript',
    'HTML': 'html',
    'CSS': 'css',
    'Swift': 'swift',
    'Go': 'golang',
    'Shell': 'shell',
    'Objective-C': 'objectivec',
    'Rust': 'rust'
  };

export interface loginModel {
    email : string,
    password : string
}


export interface registerModel {
    username : string, 
    email : string,
    password : string,
    profilePic : any
}
export interface userModel {
    username : string, 
    email : string,
    password : string,
    profilePic : any
}
export interface userBanner {
    username : string,
    profilePic : any
}

export interface projectModel {
    projectname : string,
    userid : any,
    code: any
}

export interface projectDTO {
    projectId : any,
    projectName : string,
    room :string,
    code : string
}

export interface userDTO {
    user : string,
    room : string,
    code : string
}