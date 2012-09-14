

Were you tired of trying to have to deal with a 'object' based Cache api and casting objects all over?
Yeah - me too, so enter in this simple cache provider interface.

This initial release provides basic add/remove/get/exists functionality for a memory cache.
The next release should support lambda expressions as well hopefully shortly.
The usage would then be even easier such as caching an object based on the customer id:

_cache.Add(o=>o.CustomerId)  

See the demo program for existing (non-linq) usage, but basically
_cache.Add(key, customer);
var customer = _cache.Get<Customer>(key);
