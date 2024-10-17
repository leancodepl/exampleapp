# Dedicated API project

Currently, the app has a single "domain" that maps 1-1 to an infra-level "service". This is visible
in the folder structure:

```
backend/
├─ Apps/
├─ Examples/
│  ├─ ExampleApp.Examples.Api
│  ├─ ExampleApp.Examples.Contracts
│  ├─ ExampleApp.Examples.Domain
│  ├─ ExampleApp.Examples.Services
```

The `Api` project was supposed to serve as an entrypoint to multi-domain projects. In the case of a
single service, it is redundant.

## Status

Accepted (2024-10-16)

## Context

The `Api` project is sometimes put in `Apps` folder. This is a much older approach that we move away
from.

In the case of a monolithic application, where there are multiple domains (or "areas") that are run
side-by-side in a single physical service, the `Api` project makes sense as it might work as an
entrypoint to multiple domains.

## Decision

We decided to integrate `Api` and `Services` projects into one. It will be called without a suffix,
which will help understand what it is. This results in the following folder structure for this
project:

```
backend/
├─ Apps/
├─ Examples/
│  ├─ ExampleApp.Examples
│  ├─ ExampleApp.Examples.Contracts
│  ├─ ExampleApp.Examples.Domain
```

In the case of monolithic applications, we leave `Api` project in `Apps` folder, but it should not
be named after a single domain.
