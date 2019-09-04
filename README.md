# Colleagues Spammer

This program is a joke. A funny joke to play against your annoying
colleagues when they send useless e-mails making you waste time. Make
them waste time too!

Usage: 
```
ColleaguesSpammer.exe <mail address> <number of mails> [<interval (in seconds)>]
```
It will send `<number of mails>` mails to `<mail address>`, one mail
every `<interval (in seconds)>` second(s), if specified. The content of
the mail will be in Italian.

## Apart from the joke...

This program is a sample C# application (which compiles for both `.NET
Framework` and `.NET Core`) to play around with C# 8 features,
including:
- [`MailKit`](https://github.com/jstedfast/MailKit) library to
  manipulate and send `MIME` messages
- LINQ
- Tuples and destructuring

## Contributing

Feel free to contribute to this application with your own jokes!

At the moment, the next biggest _TODO_ is localizing the content of the
mails.

If you open a pull request, please make sure you have followed the
coding conventions specified in the `.editorconfig` file.