Widgets & Styles
============

### Contents
1. [Introduction](#introduction)
2. [Using styles directly in C-Sharp](#using-styles-directly-in-c-sharp)
3. [Using styles via CSS](#using-styles-via-css)
4. [Setting sizes and positions from CSS](#setting-sizes-and-positions-from-css)
5. [Using more than one CSS file](#using-more-than-one-css-file)
6. [Subclassing Widget types](#subclassing-widget-types)
7. [Style lookup order](#style-lookup-order)
8. [Attribute reference](#attribute-reference)

### Introduction
BluEngine widgets support a cascading style sheet system, allowing you to quickly specify global style attributes that can still be customized down to the individual Widget level (much like CSS).

Each WidgetScreen contains an instance of StyleSheet; this is the master container of all states and styles applicable to widgets contained by the screen.

Each Widget also contains an instance of Style; this is an override \- it is the first point of reference when performing attribute lookups.

[Back to top](#widgets--styles)

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
Note that it is not important for purely visual style information to go in loading routines, it may be changed at any time.
This isn't necessarily the case for positioning and dimension style information, however.
To ensure all relevant style information is always applied, perform your widget creation inside `LoadWidgets()`, and NOT `LoadContent()`.

[Back to top](#widgets--styles)

### Using styles via CSS
You may also use CSS to style your UI's, which is more practical as it decouples logic code from layout information. It's also far more intuitive as many people are familiar with CSS.
To use CSS for styling and layouts, create a css file with the same name as your screen, and place
it in `Content/Styles`. For example, a subclass of `WidgetScreen` called `MyAwesomeHUD` would use a css file called `MyAwesomeHUD.css`,
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
Our `MyAwesomeHUD` class could now be changed to take advantage of the CSS, like this:
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

[Back to top](#widgets--styles)

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
Note that before setting any button attributes, a special style for the `ScreenWidget` type is defined; this Type exposes the `ref-width` and `ref-height`
properties that allow you to use absolute pixel values for dimensions. If you wish to set dimensions using CSS, this special Type must appear
before anything else in the CSS file, since internally widgets represent all positioning information as being percentages of their parent's width and height.

You may also alter these based on state:
```css
.secondButton
{
	width: 200px;
}

.secondButton:hover
{
	width: 220px;
}
```
In the example above, the value `220.0f` will automatically become the width of the button when it enters it's hover state. There is currently no way
of performing Tweening from CSS, so these state-based position and dimension changes will be instantaneous.

One side-effect of the way Widgets store positioning information internally is that for pragmatic reasons, very small values (between `-2.0f` and `2.0f`, inclusive), are considered
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
Note this is *only* for the dimension properties `left`, `top`, `width`, `height`, `bottom` and `right`.

[Back to top](#widgets--styles)

### Using more than one CSS file
You may wish to store a common set of style information in one CSS file, and have it referred to by many of your Screens, which will each load their own specific style information.
This is supported using the function `LoadCSS()`, which takes as it's only parameter a path to the CSS file you wish to load (with the path being relative to `Content/Styles`). Example:
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

[Back to top](#widgets--styles)

### Subclassing Widget types
Since a Widget's Type is an important piece of information to the StyleSheet, there are a few very important overrides for any subclass of widget to implement
for styles to function correctly.

The first is `Widget.Hierarchy`, which will return a reference to a static `List<Type>` containing the Widget hierarchy up to (and including) the current Type.
Fortunately it's pretty easy to implement. Just drop the code below into your subclass, replacing `MyCustomWidget` with the new class name:
```csharp
public override List<Type> Hierarchy
{
	get
	{
		if (hierarchy == null)
		{
			hierarchy = new List<Type>();
			hierarchy.Add(typeof(MyCustomWidget)); ///change this line!
			hierarchy.AddRange(base.Hierarchy);
		}
		return hierarchy;
	}
}
private static List<Type> hierarchy = null;
```
It might seem a bit clunky, but generating one static hierarchy for each type is much faster than doing complex reflection stuff during each call to Update()!

The second override you need to make is `Widget.CurrentState`. It should return a "live" version of the bar-delimited list of states currently being employed by your widget,
which is then assigned it to `Widget.State` at any new point in your own code where the state changes. One painless way of doing this is using related property setters,
like in this example from BluEngine's `Button` class: 
```csharp
protected override String CurrentState
{
	get { return (Enabled ? (onClick != null && mouseEntered ? (mouseDown ? "down|" : "") + "hover|" : "") : "") + base.CurrentState; }
}

public bool IsMouseEntered
{
	get { /* ... */ }
	protected set { mouseEntered = value; State = CurrentState; }
}
```
You will not need to do your own for `Control.Enabled`, `Button.IsMouseDown` and `Button.IsMouseEntered` if you're subclassing these as this logic is already implemented;
they're just used here as logic examples.

One final change you need to make is in CSS. Due to an unfortunate side-effect of how C\# does type-resolution from strings,
you need to tell BluEngine the name of the assembly in which your class is located. For example, a custom button called `AwesomeButton`,
inside namespace `MyGame.Widgets`, located in an assembly called `MyGameAssembly`, would appear in BluEngine CSS like this:
```css
#MyGame.Widgets.AwesomeButton@MyGameAssembly { ... }
```
The only reason you haven't needed to do this so far is that BluEngine assumes itself is the assembly if none is given in CSS.
For more information, see the [.NET Type.GetType() Documentation](http://msdn.microsoft.com/en-us/library/w3f99sx1%28v=vs.100%29.aspx).

[Back to top](#widgets--styles)

### Style lookup order
For visual style attributes that are referenced on-demand (image layers, alpha, etc), the Widget looks up the attributes from the style hierarchy, ensuring that the highest level containing a particular
attribute is the one used. This hierarchy is dependant on the Widget's state and type. For example, an instance of `Button` that was currently being hovered over by the mouse would search
for a visual attribute in the following order:
```csharp
//note that at this point, buttonInstance.State would return "hover|normal"
/* 1: */ buttonInstance.Style["hover"]
/* 2: */ StyleSheet[Button]["hover"]
/* 3: */ StyleSheet[Control]["hover"]
/* 4: */ StyleSheet[Widget]["hover"]
/* 5: */ StyleSheet[null]["hover"]
/* 6: */ buttonInstance.Style["normal"]
/* 7: */ StyleSheet[Button]["normal"]
/* 8: */ StyleSheet[Control]["normal"]
/* 9: */ StyleSheet[Widget]["normal"]
/*10: */ StyleSheet[null]["normal"]
```
Note that `null` is used as a Type; `StyleSheet[null]` is a reference to a base Style that is always a part of the StyleSheet and is the root Style for every Widget.
Remember that for this lookup order to function correctly in subclasses, you must ensure you have [implemented the appropriate overrides!](#subclassing-widget-types)

[Back to top](#widgets--styles)

### Attribute reference
The following is a list of all attributes currently reserved for use by BluEngine:
- **alpha** \- *expects a float between 0.0f and 1.0f*: sets the master alpha of the widget.
- **tint** \- *expects an XNA Color object*: sets the master colour the widget output is multiplied by.
- **tint-strength** \- *expects a float between 0.0f and 1.0f*: sets the percentage difference the tint colour will be from pure White (e.g. a tint of Red and tint-strength of 0.5 will give the widget a pink tint).
- **layer-N** \- *where N is an integer between 0 and 4; expects an ImageLayer object*: sets the individual image layers of the widget. In CSS you may use a url to a texture file OR any of the CSS colours; an ImageLayer with the appropriate texture information will be created regardless of the method you use. 
- **layer-N-alpha** \- *where N is an integer between 0 and 4: expects a float between 0.0f and 1.0f*: sets the individual alpha of image layer N.
- **ref-width** and **ref-height** \- *expect a float between 0.0f and 1.0f*: These are specific to the **\#BluEngine.ScreenManager.Widgets.ScreenWidget** Type and are used to set the design-time dimensions of your UI. To set this in C\#, use **Base.RefWidth** and **Base.RefHeight**.
- **left** \- *expects a float*: The X value of the Widget's left edge, in pixels.
- **top** \- *expects a float*: The Y value of the Widget's top edge, in pixels.
- **width** \- *expects a float*: The Width of the Widget, in pixels.
- **top** \- *expects a float*: The Height of the widget, in pixels.
- **right** \- *expects a float*: The X value of the Widget's right edge, in pixels. Setting this is an alternative to altering Width.
- **bottom** \- *expects a float*: The Y value of the Widget's bottom edge, in pixels. Setting this is an alternative to altering Height.

[Back to top](#widgets--styles)
