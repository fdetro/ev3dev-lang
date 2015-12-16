# Autogen scripts for ev3dev language bindings
To help us maintain our language bindings, we have written a script to automatically update sections of code according to changes in our API specification. We define code templates using [Liquid](http://liquidmarkup.org/), which are stored in the `templates` folder with the `.liquid` extension.

## Usage

### Prerequisites
- Make sure that you have cloned the repo (including submodules) and `cd`d in to the autogen directory
- You must have Node.JS and `npm` installed
- If you have not yet done so yet, run `npm install` to install the dependencies for auto-generation

### Running from the command line
If you run the script without any parameters, it will auto-generate all language groups:
```
$ node autogen.js
```

If you would like to only process a single group (as defined in `autogen-list.json`), you can provide the name of the group as a command-line parameter:
```
$ node autogen.js docs
```

## How it works
Our script searches code files for comments that define blocks of code that it should automatically generate. The inline comment tags are formatted as follows (C-style comments):

```
//~autogen test-template foo.bar>tmp
...
//~autogen
```

After the initial declaration (`~autogen`), the rest of the comment defines parameters for the generation script. The first block of text up to the space is the file name of the template to use. The `.liquid` extension is automatically appended to the given name, and then the file is loaded and parsed.

The rest of the comment is a space-separated list of contextual variables. The section before the `>` defines the source, and the section after defines the destination. The value from the source is copied to the destination, in the global Liquid context. This makes it possible to use a single template file to generate multiple classes.

##Implementation Notes
**TODO**

## Contributing to the autogen script

Just as we welcome contributions to the language bindings in this repo, we love to have people update our infrastructure. If you want to make a contribution, here are some quick tips to get started developing.

- After making a change, you should run the script and tell it to re-generate some files to make sure that your changes work as expected.
- If you are making more extensive changes, it may be helpful to create a temporary regen group with some test files to be able to manually test any new features or modifications.
- Although you can use any text editor, we recommend using [Visual Studio Code](https://code.visualstudio.com/) to edit the autogen scripts. It has great autocomplete, and their debug GUI works well with node.
 
