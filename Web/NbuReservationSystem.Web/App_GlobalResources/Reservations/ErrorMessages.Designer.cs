﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NbuReservationSystem.Web.App_GlobalResources.Reservations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NbuReservationSystem.Web.App_GlobalResources.Reservations.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Началният час трябва да е преди крайният..
        /// </summary>
        public static string BadHours {
            get {
                return ResourceManager.GetString("BadHours", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Датата на събитието не може да е в миналото..
        /// </summary>
        public static string DateExpired {
            get {
                return ResourceManager.GetString("DateExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Крайната дата трябва да е след началната..
        /// </summary>
        public static string EndDateIsBeforeStartDate {
            get {
                return ResourceManager.GetString("EndDateIsBeforeStartDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не са избрани дни за повторение на събитието..
        /// </summary>
        public static string NoSelectedRepetitionDays {
            get {
                return ResourceManager.GetString("NoSelectedRepetitionDays", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позволени са само положителни числа..
        /// </summary>
        public static string PositiveNumberIsRequired {
            get {
                return ResourceManager.GetString("PositiveNumberIsRequired", resourceCulture);
            }
        }
    }
}
