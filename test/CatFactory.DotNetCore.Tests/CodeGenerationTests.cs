﻿using System;
using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.OOP;
using Xunit;

namespace CatFactory.DotNetCore.Tests
{
    public class CodeGenerationTests
    {
        [Fact]
        public void TestContactClassGeneration()
        {
            var classDefinition = new CSharpClassDefinition
            {
                Namespace = "ContactManager",
                Name = "Contact",
            };

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.ComponentModel");

            classDefinition.Attributes.Add(new MetadataAttribute("Table", "\"Contact\""));

            classDefinition.Properties.Add(new PropertyDefinition("Int32?", "ID", new MetadataAttribute("Key")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "FirstName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "MiddleName", new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "LastName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestPersonClassGeneration()
        {
            var classDefinition = new CSharpClassDefinition
            {
                Namespace = "ContactManager",
                Name = "Person",
            };

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.ComponentModel");

            classDefinition.Attributes.Add(new MetadataAttribute("Table", "\"Persons\"")
            {
                Sets = new List<MetadataAttributeSet>()
                {
                    new MetadataAttributeSet("Schema", "\"PersonalInfo\"")
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("Int32?", "ID", new MetadataAttribute("Key")) { IsVirtual = true });
            classDefinition.Properties.Add(new PropertyDefinition("String", "FirstName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "MiddleName", new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "LastName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "Gender", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "1")));
            classDefinition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate", new MetadataAttribute("Required")));

            classDefinition.SimplifyDataTypes();

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCSharpInterfaceGeneration()
        {
            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                Namespace = "ContactManager",
                Name = "IPerson",
                Namespaces = new List<String>()
                {
                    "System",
                    "System.ComponentModel"
                }
            };

            interfaceDefinition.Properties.Add(new PropertyDefinition("Int32?", "ID"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "FirstName"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "MiddleName"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "LastName"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "FullName") { IsReadOnly = true });
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "Gender"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("Int32", "Age") { IsReadOnly = true });

            var classBuilder = new CSharpInterfaceBuilder
            {
                ObjectDefinition = interfaceDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCSharpContractInterfaceGeneration()
        {
            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                Namespace = "Contracts",
                Name = "IRepository",
                Namespaces = new List<String>()
                {
                    "System",
                    "System.Threading.Tasks"
                }
            };

            interfaceDefinition.Properties.Add(new PropertyDefinition("DbContext", "DbContext") { IsReadOnly = true });

            interfaceDefinition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChanges", new ParameterDefinition("int", "foo", "0")));

            interfaceDefinition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChangesAsync"));

            var classBuilder = new CSharpInterfaceBuilder
            {
                ObjectDefinition = interfaceDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpViewModelClassGeneration()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespace = "ViewModels";
            classDefinition.Name = "MyViewModel";

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.ComponentModel");

            classDefinition.Implements.Add("INotifyPropertyChanged");

            classDefinition.Events.Add(new EventDefinition("PropertyChangedEventHandler", "PropertyChanged"));

            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, "String", "m_firstName"));
            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, "String", "m_middleName"));
            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, "String", "m_lastName"));

            classDefinition.Constructors.Add(new ClassConstructorDefinition());
            classDefinition.Constructors.Add(new ClassConstructorDefinition());

            classDefinition.Properties.Add(new PropertyDefinition("String", "FirstName")
            {
                IsAutomatic = false,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return m_firstName;")
                },
                SetBody = new List<ILine>()
                {
                    new CodeLine("if (m_firstName != value)"),
                    new CodeLine("{"),
                    new CodeLine(1, "m_firstName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));"),
                    new CodeLine("}")
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "MiddleName")
            {
                IsAutomatic = false,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return m_middleName;")
                },
                SetBody = new List<ILine>()
                {
                    new CodeLine("if (m_middleName != value)"),
                    new CodeLine("{"),
                    new CodeLine(1, "m_middleName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MiddleName)));"),
                    new CodeLine("}")
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "LastName")
            {
                IsAutomatic = false,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return m_firstName;")
                },
                SetBody = new List<ILine>()
                {
                    new CodeLine("if (m_lastName != value)"),
                    new CodeLine("{"),
                    new CodeLine(1, "m_lastName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName));"),
                    new CodeLine("}")
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "FullName")
            {
                IsVirtual = true,
                IsAutomatic = false,
                IsReadOnly = true,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return String.Format(\"{{0}} {{1}}\", FirstName, LastName);")
                }
            });

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpViewModelClassGenerationWithExtensionMethods()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespace = "ViewModels";
            classDefinition.Name = "ProductViewModel";

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.ComponentModel");

            classDefinition.Implements.Add("INotifyPropertyChanged");

            classDefinition.Events.Add(new EventDefinition("PropertyChangedEventHandler", "PropertyChanged"));

            classDefinition.AddViewModelProperty("Int32?", "ProductId");
            classDefinition.AddViewModelProperty("String", "ProductName");
            classDefinition.AddViewModelProperty("Int32?", "ProductCategoryID");
            classDefinition.AddViewModelProperty("Decimal?", "UnitPrice");
            classDefinition.AddViewModelProperty("String", "ProductDescription");

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpRepositoryClassGeneration()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespace = "Repositories";
            classDefinition.Name = "DboRepository";

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.Collections.Generic");

            classDefinition.Implements.Add("IRepository");

            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, "DbContext", "m_dbContext"));

            classDefinition.Constructors.Add(new ClassConstructorDefinition());

            classDefinition.AddReadOnlyProperty("DbContext", "DbContext", new CodeLine("return m_dbContext;"));

            classDefinition.Properties.Add(new PropertyDefinition("IUserInfo", "UserInfo"));

            classDefinition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChangesAsync")
            {
                IsAsync = true,
                Lines = new List<ILine>()
                {
                    new CommentLine("Save changes in async way"),
                    new CodeLine("return await DbContext.SaveChangesAsync();")
                }
            });

            classDefinition.Methods.Add(new MethodDefinition("IEnumerable<TEntity>", "GetAll")
            {
                Lines = new List<ILine>()
                {
                    new TodoLine("Check generic cast"),
                    new CodeLine("return DbContext.Set<TEntity>();")
                }
            });

            classDefinition.Methods.Add(new MethodDefinition("void", "Add", new ParameterDefinition("TEntity", "entity"))
            {
                Lines = new List<ILine>()
                {
                    new CommentLine("Check entry state"),
                    new CodeLine("DbContext.Set<TEntity>().Add(entity);"),
                    new CodeLine(),
                    new TodoLine("Save changes in async way"),
                    new CodeLine("DbContext.SaveChanges();")
                }
            });

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpClassWithMethodsGeneration()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespaces.Add("System");

            classDefinition.Namespace = "Operations";
            classDefinition.Name = "Helpers";

            classDefinition.Methods.Add(new MethodDefinition("void", "Foo", new ParameterDefinition("int", "foo", "0"), new ParameterDefinition("int", "bar", "1"))
            {
                IsAsync = true
            });

            classDefinition.Methods.Add(new MethodDefinition("void", "Bar", new ParameterDefinition("int", "a")));

            classDefinition.Methods.Add(new MethodDefinition("int", "Zaz")
            {
                Lines = new List<ILine>()
                {
                    new CodeLine("return 0;")
                }
            });

            classDefinition.Methods.Add(new MethodDefinition("int", "Qux")
            {
                GenericType = "T",
                WhereConstraints = new List<String>()
                {
                    "T : class"
                },
                Lines = new List<ILine>()
                {
                    new CodeLine("return 0;")
                }
            });

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCSharpStaticClassWithExtensionMethods()
        {
            var classDefinition = new CSharpClassDefinition
            {
                Namespace = "Helpers",
                Name = "Extensions",
                IsStatic = true
            };

            classDefinition.Namespaces.Add("System");

            classDefinition.Methods.Add(new MethodDefinition("void", "Foo", new ParameterDefinition("int", "foo", "0"), new ParameterDefinition("int", "bar", "1"))
            {
                IsStatic = true,
                IsExtension = true
            });

            classDefinition.Methods.Add(new MethodDefinition("String", "Bar", new ParameterDefinition("long", "x", "0"))
            {
                IsStatic = true,
                IsExtension = true,
                GenericType = "TModel",
                WhereConstraints = new List<String>() { "TModel : class" }
            });

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCSharpClassWithFinalizer()
        {
            var classDefinition = new CSharpClassDefinition
            {
                Namespace = "Infrastructure",
                Name = "DbContext",
                IsStatic = true
            };

            classDefinition.Namespaces.Add("System.Data.Common");

            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, "IDbConnection", "Connection") { IsStatic = true });

            classDefinition.StaticConstructor = new ClassConstructorDefinition
            {
                Lines = new List<ILine>()
                {
                    new CodeLine("Connection = DbFactory.GetDefaultInstance();")
                }
            };

            classDefinition.Finalizer = new FinalizerDefinition
            {
                Lines = new List<ILine>()
                {
                    new CodeLine("Connection.Dispose();")
                }
            };

            var classBuilder = new CSharpClassBuilder
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }
    }
}
