Widget Styles
============

BluEngine widgets support a cascading style sheet system, allowing you to quickly specify global style attributes that can still be customized down to the individual Widget level (much like CSS).

Each WidgetScreen contains an instance of StyleSheet; this is the master container of all states and styles applicable to widgets contained by the screen.

Each Widget also contains an instance of Style; this is an override - it is the first point of reference when performing attribute lookups.

### Using styles in C#
Say you wanted to create a set of buttons that each mostly appeared the same (background colour, hover colour, etc.), but had a different image inside and one had a different hover colour:
```c-sharp
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

You may also use CSS to style your UI's, which is more practical as it decouples logic code from layout information. It's also far more intuitive as many people are familiar with CSS.
To use CSS for styling and layouts, create a css file with the same name as your WidgetScreen subclass's Type, and place
it in Content/Styles. For example, a subclass of WidgetScreen called MyAwesomeHUD would use a css file called MyAwesomeHUD.css,
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
Our MyAwesomeHUD.css class could now be changed to take advantage of the CSS, like this:
```c-sharp
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

#### General attributes:
- **alpha** - *expects a float between 0.0f and 1.0f*: ; sets the master alpha of the widget.
- **tint** - *expects an XNA Color object*: ; sets the master colour the widget output is multiplied by.
- **tint-strength** - *expects a float between 0.0f and 1.0f*: ; sets the percentage difference the tint colour will be from pure White (i.e. a tint of Red and tint-strength of 0.5 will end up a light pink-red color).
- **layer-N** - *where N is an integer between 0 and 4; expects an ImageLayer object*: sets the individual image layers of the widget.
- **layer-N-alpha** - *where N is an integer between 0 and 4; expects a float between 0.0f and 1.0f*: sets the individual alpha of image layer N.

#### Type-specific attributes:
- **ref-width** - *expects a float between 0.0f and 1.0f*
