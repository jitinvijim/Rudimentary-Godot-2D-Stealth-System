using Godot;
using System.Collections.Generic;
using System.Data;


public partial class EnemyFSM : Node
{
    private Dictionary<string, State> _states;
    private State _currentState;

    public override void _Ready()
    {
        _states = new Dictionary<string, State>(); //like HashMap, key being name of the node, value being actual State Node.
        foreach(Node node in GetTree().GetNodesInGroup("FSM States"))
        {
            if(node is State s) //if node extends from State
            {
                _states[node.Name] = s; //add the state with by its node's name
                s.enemyFSM = this; //this is the class that is to be referenced by that variable
                s.Ready(); //runs each node's ready() function
                s.Exit();
            }
        }

        foreach(string key in _states.Keys)
        {
            GD.Print(key);
        }

        _currentState = _states["Patrolling"];
        _currentState.Enter();
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState.PhysicsUpdate(delta);
    }

    public void TransitionTo(string nextState)
    {
        if(_states[nextState] == _currentState)
            {
                GD.Print("next state ", nextState, "is the same as current state", _currentState.Name);
                return;
            }
        else if(!_states.ContainsKey(nextState))
            {
                GD.Print("there is no state called ", nextState);
                return;
            }
        
        _currentState.Exit();
        _currentState = _states[nextState];
        _currentState.Enter();
    }

}
