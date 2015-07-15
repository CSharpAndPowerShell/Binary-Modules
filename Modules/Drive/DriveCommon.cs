
namespace Drive
{
    public class DriveCommon
    {
        private Shell32.Shell shell = new Shell32.Shell();
        public void RenameDrive(char letter, string name) {
            //Renombra unidades locales y de red
            ((Shell32.Folder2)shell.NameSpace(letter + ":")).Self.Name = name;
        }
    }
}
