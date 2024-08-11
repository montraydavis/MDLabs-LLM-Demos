using System;


public enum HttpMethod{
    GET,
    POST,
    PUT,
    DELETE
}

public class RouteAttribute(string route, HttpMethod method) : Attribute
{

    public string Route => route;
    public HttpMethod Method => method;
}