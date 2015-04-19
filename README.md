### Overview
Impostor is a HTTP service (Owin middleware) that provides a way to mock another web service.
It is in a very raw state at the moment, and its internals and APIs are very likely to change.

It is not on NuGet yet.

### Usage
You can run Impostor using `Impostor.Console.exe` (this will use `Settings.yaml` in the folder).

You can also include Impostor middleware in some other project:
```csharp
app.UseImpostor(settings)
```

In that case, you can use `Impostor.Settings.Yaml.YamlSettingsParser` to get the settings from a YAML file.

### Settings

_Note: This is version 0.1.0 and so this is very likely to change._

```yaml
# identifies path that be used to log all requests received by Impostor
request_log: .\Requests\{$now:yyyy-MM-dd--HH-mm-ss}.txt

# List of rules that define how requests would be handled
rules:

    # Path to match -- wildcards and proper routing are not supported yet
  - url:    /test

    # Inline response definition. Can only be specified if 'response_path' was not specified.
    response:
      # Response status code
      status: 200

      # Response content type
      type: text/json

      # File to use as the response body (inline body not supported yet)
      body_path: body.txt
      
    # Path to response definition. Can only be specified if 'response' was not specified.
    response_path: response.txt
```

#### Substitutions
Currently substitutions are only supported in `request_log`.
Later I plan to extend that to all other setting elements and response files.

Substitution format is `{name:format}`. Currently only the following are supported:

| Name | Value |
| ------------- |-------------|
| $now | `DateTime.Now` |
| $guid | `Guid.NewGuid()` |

Example: `request_log: .\Requests\{$now:yyyy-MM-dd--HH-mm-ss}.txt`.

#### Response files
If `response_path` is used instead of inline `response`, the response file is expected to have the following format:
```
200 OK

Content-Type: text/json
Some-Header-1: SomeValue1
Some-Header-2: SomeValue2

{ "key": "wow" }
```

Headers and body are optional. If headers are omitted, there should be two empty lines between the status and the body.