﻿using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class CSharpClassDefinition : ClassDefinition, IDotNetClassDefinition
    {
        public CSharpClassDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }

        public ICodeNamingConvention NamingConvention { get; } = new DotNetNamingConvention();

        public ITypeResolver TypeResolver { get; protected set; }
    }
}
