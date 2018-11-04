using System.Windows.Forms;

namespace Host.Utils
{
    public static class Message
    {
        #region Public Methods

        public static void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Host Error");
        }

        #endregion
    }
}
