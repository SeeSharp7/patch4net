# patch4net
A free and easy to use .NET library for JSON PATCH (RFC 6902) and JSON MERGE PATCH (RFC 7386) in your RESTful services.

## IMORTANT NOTICE!
JSON PATCH (RFC 6902) is currently not implemented! If this is a essential feature for you, wait until it's implemented.

## INSTALLATION
This should be published via the NuGet Package Manager. 

## DEPENDENCIES
This library depends on Newtonsoft.Json aka Json.Net. The dependency is resolved by a dependent NuGet-Package. No binary references!

## USAGE
The usage of this library is very simple. It only consumes the minimum required data. There is no restriction on your original model and it doesn't need to be converted because of generic types. The body of your incoming patch request simply needs to be a string.

### Identify Patch-variant via `ContentType`-Header
You want to support json patch as well as json merge patch in your API? No problem. The variant of patch must be set in the `ContentType`-Header. When you make use of the `UniversalPatch`-Method, you don't even need to check the header. patch4Net will do that for you and calls the right patch implementation!

```csharp
var patchedModel = new JsonPatcher().UniversalPatch(requestBody, originalModel, incomingContentTypeHeaderValue);
```

### JSON PATCH (RFC 6902)
```csharp
var patchedModel = new JsonPatcher().Patch(patchJson, simpleModel);
```

### JSON MERGE PATCH (RFC 7386)
```csharp
var patchedModel = new JsonPatcher().MergePatch(mergePatchJson, simpleModel);
```

## CUSTOMIZATION
Many projects make use of exotic settings within their serializers. So it's impossible to write one, that covers all needs. To solve that problem, I decided to create an interface `ISerializer` to implement completely custom serializers with the specific settings.

The Interface provides two simple methods:
```csharp
TModelType DeserializeObject<TModelType>(string serializedObject);
TModelType DeserializeObject<TModelType>(string serializedObject);```
