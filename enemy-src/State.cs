using Godot;
using System;

public partial class State : Node
{
	public EnemyFSM enemyFSM;  
	//will need to be initialized, use enemyFSM = this
	//to let the compiler know that that class is the class that 
	//the above line is referencing 
	public virtual void Enter() {}
	public virtual void Exit() {}

	public new virtual void Ready() {}
	public virtual void PhysicsUpdate(double delta) {}
	

	
}
