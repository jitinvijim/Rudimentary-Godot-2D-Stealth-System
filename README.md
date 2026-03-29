#Simple 2D stealth system on Godot (C#) that you can use on your own projects.

##Features
- Templated for easy plugging in.
- Most properties can be edited in-engine, keeping it easy and usable.
- Modular, can be reused for several mob entities. Can also be edited easily to have more features
- Relatively low cost, but accurate and robust.
- Reset Scene button for sanity. Can easily be removed

##How it works
Uses Area2D, combined with RayCast2D, both efficient and low cost. Area2D for a Vision Cone, RayCast2D for Line-Of-Sight

##How to use
- In your Enemy scene, create a child Node called EnemyStealth (this name can be changed to anything, edited in [Enemy.cs](https://github.com/jitinvijim/Rudimentary-Godot-2D-Stealth-System/blob/6dd335ab427c9378789824398371acbff618e570/enemy-src/Enemy.cs#L18) and [EnemyStealth.cs](https://github.com/jitinvijim/Rudimentary-Godot-2D-Stealth-System/blob/6dd335ab427c9378789824398371acbff618e570/enemy-src/EnemyStealth.cs#L4).
- Add a child RayCast2D node to your EnemyStealth node, call it LOS ([EnemyStealth.cs](https://github.com/jitinvijim/Rudimentary-Godot-2D-Stealth-System/blob/6dd335ab427c9378789824398371acbff618e570/enemy-src/EnemyStealth.cs#L43).
- Attatch a the enemyStealth.cs file to the EnemyStealth node. Godot requires the Class name to be the same as the Node name, so make sure it's the same.
- Within the Enemy scene again child Area2D node called VisionCone. create two child nodes underneath, called VisionConeVisual (Polygon2D) and VisionConeCollisionShape (CollisionPolygon2D).
  VisionConeVisual gives it the colour, while VisionConeCollisionShape gives it its shape. When editing its shape, just change VisionConeCollisionShape, VisionConeVisual inherits from it.
- Attatch the visionCone.cs file to the Area2D VisionCone node. again with the Class name being the same as Node name
- Now within your main scene, create an Enemy instance, and have its Path be a sibling node. Once you build the scene, in the Inspector you will be asked to attach a PathFollow2D node. 
- There is some scripting in main.cs that is required. Attach a script file. From this project's main.cs you don't need lines 6, 13, 21 to 29 (for the Detected/Undetected Debug Text) and lines 31 to 34 (Reload Scene button).
- Finally, add the Player node to a Group called "Player" (can be changed in [EnemyStealth.cs](https://github.com/jitinvijim/Rudimentary-Godot-2D-Stealth-System/blob/9b2c648cafc7cc3cc57685ce708be8018d02a902/enemy-src/EnemyStealth.cs#L90).
- You should be all set. explore the Inspector for some useful exported parameters, build, and enjoy!

  Alternatively of course you could just copy-paste the entire thing

###Please do raise issues if ever any arise.



##Thanks! And Happy Building!
