﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoCAD.SQL.Plugin
{
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.11.0.0")]
    internal sealed partial class SettingsDb : global::System.Configuration.ApplicationSettingsBase {
        
        private static SettingsDb defaultInstance = ((SettingsDb)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SettingsDb())));
        
        public static SettingsDb Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server=STZ-LAP-ETM001\\\\SQLEXPRESS;Database=AUTOCADDB;Integrated Security=True;Tru" +
            "stServerCertificate=Yes")]
        public string connectionString {
            get {
                return ((string)(this["connectionString"]));
            }
        }
    }
}
