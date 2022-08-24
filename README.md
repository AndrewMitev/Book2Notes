# Book2Notes
WPF windows desktop application that performs validations and editing of .exp - Export Library Files

## Purpose of this convertor

Utf-8 text files in specific format (here called Book.Exp format) consists of an export of books in digital format taken from a biblesoftware program. The files need to be converted to another format, which is a variant of this Book.Exp format. The converted format does have the name Notes.Exp format.

![Alt text](logo.png?raw=true "Logo")

## Charachteristics of the import file
UTF-8 tekst file with BOM
File is full of tekst (including Greek and Hebrew) and codings. For our purpose one particular coding is of importance:

```bash
$$$ n <date-time>
```
It always starts on a new line with three times $, followed by a space and then a number of one or more digits (maximum number is 32000). Optional there is a <date-time> stamp after the number, seperated from the number with a space.
There is never information after this coding
Under item 0 there is some speciale information, describing the content of the file
It looks like:

```python
$$$ 0
$Index
00000 Title
00000 
00001 Titlepage
00002 Content 1
00003 Content 2
$$$ 1
\$Titlepage\$
… tekst
$$$ 2
\$ Content 1\$
… tekst
$$$ 3
\$Content 2\$
… tekst
$$$ 4
…
```
This index to the whole file is described as follows:
Starting line with $Index to identify that the following lines contain an index.
The next lines always start with 5 digits followed by a space and then a description of the content.
The five digit-number identifies the item-number. So  00001 refers to the content which starts under $$$ 1 and ends before $$$ 2 (or the end of the file), etc.

## Characteristics of the output Notes.Exp file
The output file (after conversion is complete) consists of the following characteristics:
$$$ 0  does not exsist
The whole Index content is removed
The item-number after $$$ is replaced by another type of label, i.e. $$$ Bb 1:1
The syntaxt for this new label is:
$$$ = the same code as with the Book.Exp
Bb = abbreviation of Biblebook (this is alfanumerical – like Ge, 1Ch, 1Cor, Ga, Heb, Jas, 2Pe, 3Jo, Re)
1 = chapter number (always in between 1 and 150)
: = colon as chapter:verse seperator
1 = verse number (always in between 1 and 177)
So the output of the above input looks like:
```bash
$$$ Ge 1:1
\$Titlepage\$
… tekst
$$$ Ge 1:2
\$ Content 1\$
… tekst
$$$ Ex 3:7
\$Content 2\$
… tekst
$$$ Le 5:39
…
```
## How the conversion is managed
Before the convertor can be used there is needed editorial work. Someone has to decide what the new position becomes in the Notes.Exp and which information is used and which should be left out.
For this reason the whole item 0 under $Index is edited in the following way:
```bash
$$$ 0
$Index
00000 Title
00000 
00001 <DEL> Titlepage
00002 <Ge 1:1> Content-title 1
00003 <Ge 1:2> Content-title 2
00003 <CONT> SubContent-title 2a
00004 <Ge 1:3> SubContent-title 2b
00005 <DEL>Interlude
00006 <Ex 2:1> Content-title 3
00007 <RL> Content-title 4
```

In this example you see that special information is inserted direct after the five digit number.
This information is interpreted in the following way:
After 00000 there is no conversion information, so under item 0 nothing happens with the $$$.
After 00001 there is a DEL which means that item 1 AND its content needs to be removed.
After 00002 there is <Ge 1:1> which means that $$$ 2 <date-time> is replaced by $$$ Ge 1:1
After 00003 there is <Ge 1:2> which means that $$$ 3 <date-time> is replaced by $$$ Ge 1:2
After 00003 there is <CONT>, this line is simply ignored. The reason: 00003 was in the line before as well, but now with a conversion command. So if a five digit number is mentioned for a second time there should be a <CONT> command  (important for error-checking)
After 00004 there is <Ge 1:3> which means that $$$ 4 <date-time> is replaced by $$$ Ge 1:3
After 00005 there is a DEL which means that item 5 AND its content needs to be removed.
After 00006 there is <Ex 2:1> which means that $$$ 6 <date-time> is replaced by $$$ Ex 2:1
After 00007 there is RL (= Remove Label) which means that the line $$$ 7 <date-time> needs to be replaced by a cr lf.
The same as with the CONT command (including error-checking)
## Error-checking
When no errors are found the checking ends with the message “No errors found” and OK to remove the message.
When errors are found the message appears “Errors in file Error_date_time.txt”
The errors are written in a tekst file with the name to which a date_time stamp is added in the filename. This errorfile is in the same folder as the input file.
The error checking check for the following problems
1.	All $$$ n items must be mention as a five digitnumber under $$$ 0. If not it reports “$$$ n not mentioned in index”
2.	All five-digit itemnumbers under $$$ 0 must be available as an $$$ n item in the file. If not it reports “nnnnn is not available in the file”.
3.	For the commands CONT there should be available a normal conversion string for that same five-digitnumber or a RL-command
4.	There must be a command for every available fivedigit number under item 0, except for item 0.


## Support
Further development of application is expected with updating and improving existing functionality as well as adding new modules for additional business requirements.

## License
[MIT](https://choosealicense.com/licenses/mit/)
