# AetheBot 2: Electic Boogaloo for Discord

## Setup

Create a Discord Bot account
[here](https://discordapp.com/developers/applications/me).

Copy the token it generated.

Right click project in Visual Studio -> Manage User Secrets

Add to the file like this:

```json
{
  "DISCORD_TOKEN": "[token]"
}
```

## Debugging in Visual Studio

### Without Docker

You should be able to debug by hitting F5 which will spin up a console app.

### Running Node Inside Docker

TBD

## Environment Variables

| Variable             | Required? | Purpose | Example |
| -------------------- | --------- | ------- | ------- |
| `DISCORD_TOKEN`      | Yes       | The generated Discord Bot account token for authentication with Discord. | `aaabbbccc111222333`|
| `WEBSITE_BASE_URL`   | No        | The base URL for the bot's internal website. Leave undefined to not run the website.| `https://my-great-aethebot-instance.herokuapp.com`
| `DEFAULT_ADMIN_USER` | No        | Discord User ID for the default admin user. | `12345678901234567` |
| `REDIS_URL`          | No        | URL to your Redis instance, to use as the bot's persistent storage. | `redis://user:password@my-redis.host:13337`|

## Creating a Voice Noise

The voice noises themselves are in an object in
`TBD`. Provide the file name for the noise to
play, and a regular expression to match on. It will play the noise in the
channel you are currently in if the regex matches and you've mentioned the bot.
Please keep try to keep the volume of your noise consistent with the other
noises.

Noises can be provided in `.opus` format, 64kbps mono. Please listen to some of
the existing voice noises and adjust the volume accordingly.
