using System.Collections.Generic;

public abstract class Command
{
	private static readonly Queue<Command> _commands = new Queue<Command>();

	private static readonly Stack<Command> _history = new Stack<Command>();

	public static void QueueCommand(Command command)
	{
		_commands.Enqueue(command);
	}

	public static void ProcessCommands()
	{
		while (_commands.Count > 0)
		{
			var command = _commands.Dequeue();
			command.Execute();
			_history.Push(command);
		}
	}

	public static void Undo()
	{
		if(_history.Count > 0)
		{
			var command = _history.Pop();
			command.Execute();
		}
	}

	protected abstract void Execute();
	protected abstract void Rollback();
}

// public class Move : Command
// {
// 	private Vector3 _moveTo;
// 	private Vector3 _moveFrom;
// 	private GameObject _object;

// 	public Move (GameObject obj, Vector3 destination)
// 	{
// 		_moveTo = destination;
// 		_object = obj;
// 	}

// 	protected override void Execute()
// 	{
// 		_moveFrom = _object.transform.position;
// 		_object.transform.position = _moveTo;
// 	}
// }