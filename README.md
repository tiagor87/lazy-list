[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=tiagor87_lazy-list&metric=alert_status)](https://sonarcloud.io/dashboard?id=tiagor87_lazy-list)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=tiagor87_lazy-list&metric=coverage)](https://sonarcloud.io/dashboard?id=tiagor87_lazy-list)

# LazyList

Simple library to load a list just when is necessary.

## How to use

```csharp
services.AddLazyList(Assembly);
```

Implements list resolvers.

```csharp
public class AddressListResolver : LazyLoadResolver
{
    private IAddressProvider _provider;

    public AddressListResolver(IAddressProvider provider) : base(typeof(Address))
    {
        _provider = _provider;
    }
    
    protected override async Task<object> LoadAsync(LazyLoadParameter parameter)
    {
        return await _provider.GetByPersonAsync((Person) parameter.Value);
    }
}
```

Instantiate LazyList.

```csharp
LazyListFactory.CreateList<Address>(person);
```