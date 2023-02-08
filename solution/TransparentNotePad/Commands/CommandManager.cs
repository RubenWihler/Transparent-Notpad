using System.Collections.Generic;

namespace TransparentNotePad.Commands
{
    public class CommandManager
    {
        private readonly Stack<ICommand> _undo_commands;
        private readonly Stack<ICommand> _redo_commands;

        public CommandManager()
        {
            this._undo_commands = new Stack<ICommand>();
            this._redo_commands = new Stack<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            _undo_commands.Push(command);
            command.Execute();
        }

        public void Undo()
        {
            if (_undo_commands.Count == 0) return;

            var command = _undo_commands.Pop();
            command.Undo();
            _redo_commands.Push(command);
        }
        public void Redo()
        {
            if (_redo_commands.Count == 0) return;

            var command = _redo_commands.Pop();
            command.Execute();
            _undo_commands.Push(command);
        }
        
        public void ClearCommands()
        {
            _undo_commands.Clear();
            _redo_commands.Clear();
        }
    }
}
