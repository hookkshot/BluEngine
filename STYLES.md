Widget Styles
============

### The following attributes are currently supported by the widget style system:
- **alpha** - *expects a float between 0.0f and 1.0f*: ; sets the master alpha of the widget.
- **tint** - *expects an XNA Color object*: ; sets the master colour the widget output is multiplied by.
- **tint-strength** - *expects a float between 0.0f and 1.0f*: ; sets the percentage difference the tint colour will be from pure White (i.e. a tint of Red and tint-strength of 0.5 will end up a light pink-red color).
- **layer-N** - *where N is an integer between 0 and 4; expects an ImageLayer object*: sets the individual image layers of the widget.
- **layer-N-alpha** - *where N is an integer between 0 and 4; expects a float between 0.0f and 1.0f*: sets the individual alpha of image layer N.
