﻿using System;
using System.Diagnostics;
using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpInterfaceDefinition : InterfaceDefinition, IDotNetInterfaceDefinition
    {
        public CSharpInterfaceDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICodeNamingConvention m_namingConvention;

        [Obsolete("Set instance for ICodeNamingConvention in CodeBuilder instance")]
        public ICodeNamingConvention NamingConvention
        {
            get
            {
                return m_namingConvention ?? (m_namingConvention = new DotNetNamingConvention());
            }
            set
            {
                m_namingConvention = value;
            }
        }
    }
}
