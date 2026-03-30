using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace ViertelStdTool.Views
{
    /// <summary>
    /// Interaktionslogik für UserInfoEmergencySoftware.xaml
    /// </summary>
    public partial class UserInfoEmergencySoftware : MetroWindow
    {
        #region constructor
        internal UserInfoEmergencySoftware()
        {
            // Do basic initialization.
            InitializeComponent();           
        }
        #endregion

        #region button - click event for close window
        private void UseTool_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            this.Close();
        }
        #endregion

        #region button - click event for close window
        private void CloseTool_Click(object sender, RoutedEventArgs e)
        {
            // Close Application
            Environment.Exit(0);
        }
        #endregion
    }
}
