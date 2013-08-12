Widgets & Styles
============

BluEngine widgets support a cascading style sheet system, allowing you to quickly specify global style attributes that can still be customized down to the individual Widget level (much like CSS).

Each WidgetScreen contains an instance of StyleSheet; this is the master container of all states and styles applicable to widgets contained by the screen.

Each Widget also contains an instance of Style; this is an override - it is the first point of reference when performing attribute lookups.

### Using styles directly in C-Sharp
Say you wanted to create a set of buttons that each mostly appeared the same (background colour, hover colour, etc.), but had a different image inside and one had a different hover colour:
```csharp
//somehere in your subclass of WidgetScreen...
protected override void LoadWidgets()
{
	base.LoadWidgets();
	
	//global styles using the Button type
	Styles[typeof(Button)]["normal"]["layer-0"] = ImageLayer.FromColor(Color.Gray); //bottom layer
	Styles[typeof(Button)]["hover"]["layer-1"] = ImageLayer.FromColor(new Color(0,255,255,50)); //semi-transparent hover highlight layer
	Styles[typeof(Button)]["down"]["layer-0"] = ImageLayer.FromColor(Color.White); //this layer-0 will override the "normal" one when the button is being clicked
	
	//create the buttons
	Button playButton = new Button(Base);
	Button settingsButton = new Button(Base);
	Button exitButton = new Button(Base);

	//attach the button-specific images
	playButton.Style["normal"]["layer-2"] = new ImageLayer(Content.Load<Texture2D>("Content\\Textures\\play.png"));
	settingsButton.Style["normal"]["layer-2"] = new ImageLayer(Content.Load<Texture2D>("Content\\Textures\\settings.png"));
	exitButton.Style["normal"]["layer-2"] = new ImageLayer(Content.Load<Texture2D>("Content\\Textures\\exit.png"));

	//give the exit button a different hover colour
	exitButton.Style["hover"]["layer-1"] = ImageLayer.FromColor(new Color(255,0,0,50));
}
```
Note that it is not important for style information to go in loading routines, it may be changed at any time. You may change global styles AFTER creating your buttons too; the order of these operations is unimportant.

### Using styles via CSS
You may also use CSS to style your UI's, which is more practical as it decouples logic code from layout information. It's also far more intuitive as many people are familiar with CSS.
To use CSS for styling and layouts, create a css file with the same name as your screen, and place
it in **Content/Styles**. For example, a subclass of **WidgetScreen** called **MyAwesomeHUD** would use a css file called **MyAwesomeHUD.css**,
and to achieve the same results as the above example, it might look like this:
```css
#BluEngine.ScreenManager.Widgets.Button
{
	layer-0: gray; /* any of the CSS colour types is supported, like keywords... */
}

#BluEngine.ScreenManager.Widgets.Button:hover
{
	layer-1: #00FFFF; /* ...hex... */
}

#BluEngine.ScreenManager.Widgets.Button:down
{
	layer-1: rgba(50,50,50); /* ...rgb()/rgba()... */
}

.playButton
{
	layer-2: url("Content/Textures/play.png");
}

.settingsButton
{
	layer-2: url("Content/Textures/play.png");
}

.exitButton
{
	layer-2: url("Content/Textures/play.png");
}

.exitButton:hover
{
	layer-1: hsl(0,100%,50%); /* ... and hsl(). */
	layer-1-alpha: 0.5; /*since hsl has no alpha*/
}
```
Our **MyAwesomeHUD** class could now be changed to take advantage of the CSS, like this:
```csharp
protected override void LoadWidgets()
{
	base.LoadWidgets();

	//button creation is the same
	Button playButton = new Button(Base);
	Button settingsButton = new Button(Base);
	Button exitButton = new Button(Base);

	//only now we tell the engine which styles to attach the buttons to, instead of directly assigning the attributes:
	RegisterCSSClass("playButton", playButton);
	RegisterCSSClass("settingsButton", settingsButton);
	RegisterCSSClass("exitButton", exitButton);
}
```
Note this time around we didn't touch the global styles; this is because the CSS ID rulesets are interpreted for Types, and automatically applied to the global styles.

### Setting sizes and positions from CSS
You may also set the position and size properties of UI elements from CSS classes, like this:
```css
#BluEngine.ScreenManager.Widgets.ScreenWidget
{
	ref-width: 1280px;
	ref-height: 720px;
}

.firstButton
{
	top: 100px;
}

.secondButton
{
	top: 200px;
}

.firstButton, .secondButton
{
	left: 0px;
	width: 200px;
	height: 40px;
}
```
Note that before setting any button attributes a special style for the ScreenWidget type is defined; this Type exposes the **ref-width** and **ref-height**
properties that allow you to use absolute pixel values for dimensions. If you wish to set dimensions using CSS, this special Type must appear
before anything else in the CSS file since internally widgets represent all positioning information as being percentages of their parent's width and height.

Another limitation is that you may not use different dimensions for different states:
```css
.secondButton
{
	top: 200px; /* this will work... */
}

.secondButton:hover
{
	top: 220px; /* ...but this will not! */
}
```
In the example above, the value **220.0f** will be stored in **secondButton.Style\["hover"\]\["top"\]**, but will not be used automatically during widget setup, nor will
it be used when the state changes (since **Widget.State** is a purely visual property). You may still refer to it yourself in code, of course.

Another side-effect of the internal percentage representation is that for pragmatic reasons, very small values (between **-2.0f** and **2.0f**, inclusive), are considered
to be percentages for dimension properties, and anything outside this range is considered to be an absolute value. In the case of percentages in CSS (i.e. values with an explicit
**%** symbol), these are translated to floats by the CSS parser first, *then* passed to the style system, so this limitation works in reverse, too. This means that:
```css
.exampleClass
{
	/* these are the same: */
	top: 4000%;
	top 40px;
	
	/* ...so are these: */
	left 0.5px;
	left: 50%;
}
```
Note this is *only* for the dimension properties **left**, **top**, **width**, **height**, **bottom** and **right**.

### Using more than one CSS file
You may wish to store a common set of style information in one CSS file, and have it referred to by many of your Screens, which will each load their own specific style information. This is supported. Example:
```csharp
public class StyledScreen : WidgetScreen
{
	public override void LoadContent()
	{
		// LoadWidgets() is called within WidgetScreen.LoadContent()
		base.LoadContent();
		
		//... so as long as you override LoadWidgets() to create your widgets, and NOT LoadContent(),
		//you can be certain that the following line(s) will apply to all widgets:
		LoadCSS("common.css");
		LoadCSS("more_common.css");
		LoadCSS("holy_crap_just_merge_these_files_already.css");
	}
}

//the child classes do not need to make any calls to LoadCSS(),
//as they will invoke SomeAwesomeScreen.css and SomeOtherAwesomeScreen.css automatically.
public class SomeAwesomeScreen : StyledScreen
{
	protected override void LoadWidgets()
	{
		base.LoadWidgets();
		//create your widgets...
	}
}

public class SomeOtherAwesomeScreen : StyledScreen
{
	protected override void LoadWidgets()
	{
		base.LoadWidgets();
		//create your widgets...
	}
}
```

### Style lookup order
For visual style attributes that are referenced on-demand (image layers, alpha, etc), the Widget looks up the attributes from the style hierarchy, ensuring that the highest level containing a particular
attribute is the one used. This hierarchy is dependant on the Widget's state and type. For example, a subclass of Button, **AwesomeButton**, that was currently being hovered over by the mouse would search
for a visual attribute in the following order:
```csharp
/* 1*/ buttonInstance.Style["hover"]
/* 2*/ StyleSheet[AwesomeButton]["hover"]
/* 3*/ StyleSheet[Button]["hover"]
/* 4*/ StyleSheet[Widget]["hover"]
/* 5*/ StyleSheet[null]["hover"]
/* 6*/ buttonInstance.Style["normal"]
/* 7*/ StyleSheet[AwesomeButton]["normal"]
/* 8*/ StyleSheet[Button]["normal"]
/* 9*/ StyleSheet[Widget]["normal"]
/*10*/ StyleSheet[null]["normal"]
```
Note that **null** is used as a Type; StyleSheet\[null\] is a reference to a base Style that is always a part of the StyleSheet and is the root Style for every Widget.

### Subclassing Widget types
Since a Widget's Type is an important piece of information to the StyleSheet, there are two overrides that are very important for any subclass of widget to implement. Again, using our example widget **AwesomeButton**:
```csharp
// each Widget type has it's own static list representing it's hierarchy,
// so we don't have to do slow reflection stuff. Drop the code below into any subclass
// you make:
public override List<Type> Hierarchy
{
	get
	{
		if (hierarchy == null)
		{
			hierarchy = new List<Type>();
			hierarchy.Add(typeof(AwesomeButton)); ///change this line!
			hierarchy.AddRange(base.Hierarchy);
		}
		return hierarchy;
	}
}
private static List<Type> hierarchy = null;

// for styling purposes, State is a bar-delimited list of states in descending order of preference,
// with the first state in the list being the actual current state of the widget, or whichever is most
// important. This is how the state part of the search hierarchy is determined.

// Your actual state needs will be different than this example, but just ensure you
// include other states your class actually uses in the state list, in the right order. 
public override String State
{
	get
	{
		return (!Enabled ? "disabled|" : (mouseover ? "hover|" : "") ) + "normal";
	}
}
```

### Visual attribute reference
These are the on-demand style attributes that are supported on both the C\# AND CSS sides, for every Type and State: 
- **alpha** \- *expects a float between 0.0f and 1.0f*: sets the master alpha of the widget.
- **tint** \- *expects an XNA Color object*: sets the master colour the widget output is multiplied by.
- **tint-strength** \- *expects a float between 0.0f and 1.0f*: sets the percentage difference the tint colour will be from pure White (e.g. a tint of Red and tint-strength of 0.5 will give the widget a pink tint).
- **layer-N** \- *where N is an integer between 0 and 4; expects an ImageLayer object*: sets the individual image layers of the widget. In CSS you may use a url to a texture file OR any of the CSS colours; an ImageLayer with the appropriate texture information will be created regardless of the method you use. 
- **layer-N-alpha** \- *where N is an integer between 0 and 4: expects a float between 0.0f and 1.0f*: sets the individual alpha of image layer N.

### Dimensional attribute reference
These are the **normal-state-specific** attributes that are used for initialization from CSS only: 
- **ref-width** and **ref-height** \- *expect a float between 0.0f and 1.0f*: These are specific to the **\#BluEngine.ScreenManager.Widgets.ScreenWidget** Type and are used to set the design-time dimensions of your UI. To set this in C\#, use **Base.RefWidth** and **Base.RefHeight**.
- **left** \- *expects a float*: The X value of the Widget's left edge, in pixels.
- **top** \- *expects a float*: The Y value of the Widget's top edge, in pixels.
- **width** \- *expects a float*: The Width of the Widget, in pixels.
- **top** \- *expects a float*: The Height of the widget, in pixels.
- **right** \- *expects a float*: The X value of the Widget's right edge, in pixels. Setting this is an alternative to altering Width.
- **bottom** \- *expects a float*: The Y value of the Widget's bottom edge, in pixels. Setting this is an alternative to altering Height.

