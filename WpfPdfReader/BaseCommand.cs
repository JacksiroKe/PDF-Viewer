using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfPdfReader
{
    public abstract class BaseCommand : ICommand
    {
        private static Dictionary<ModifierKeys, string> modifierText = new Dictionary<ModifierKeys, string>()
        {
            {ModifierKeys.None,""},
            {ModifierKeys.Control,"Ctrl+"},
            {ModifierKeys.Control|ModifierKeys.Shift,"Ctrl+Shift+"},
            {ModifierKeys.Control|ModifierKeys.Alt,"Ctrl+Alt+"},
            {ModifierKeys.Control|ModifierKeys.Shift|ModifierKeys.Alt,"Ctrl+Shift+Alt+"},
            {ModifierKeys.Windows,"Windows+"}
        };

        private static Dictionary<Key, string> keyReplacements = new Dictionary<Key, string>()
        {
            {Key.Add, "+"},
            {Key.Subtract, "-"}
        };

        public string Name { get; private set; }
        public string GestureText { get; private set; }
        public InputBinding InputBinding { get; set; }

        protected BaseCommand(string name, InputGesture inputGesture)
        {
            this.Name = name;

            if (inputGesture != null)
            {
                this.InputBinding = new System.Windows.Input.InputBinding(this, inputGesture);

                if (inputGesture is KeyGesture)
                {
                    var kg = (KeyGesture)inputGesture;
                    var keyText = keyReplacements.ContainsKey(kg.Key) ? keyReplacements[kg.Key] : kg.Key.ToString();

                    this.GestureText = modifierText[kg.Modifiers] + keyText;
                }
            }
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
