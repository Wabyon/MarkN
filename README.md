MarkN
=====

A microservice that renders markdown(GFM) document to html, powered by NancyFx

## Features

* Sanitization
* Syntax highlighting

## Usage

```
POST /markdown
```

#### Paramteres

|Name|Type|Description|
|:---|:--:|:----------|
|text|string|**Required**|
|sanitize|bool|Default: true|

#### Example

``` json
{
  "text": "#Hello world\n[LinkText](http://linkpage)",
  "sanitize": true
}
```

#### Responce

``` html
Status: 200 OK
Content-Type: text/html

<h1>Hello world</h1>
<p><a href="http://linkpage">LinkText</a></p>
```
