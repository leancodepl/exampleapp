# Handlers naming conventions

In the past, we had different naming conventions for command/query handlers and event handlers (also
known as `IConsumer`s). We decided to use one standard pattern in this project.

## Status

Accepted (2024-10-16)

## Context

Different projects currently use different strategies for storing handlers. The most commonly found
are:

1. The `CQRS` folder with command and query handlers, grouped in areas; `Processes` with event
   handlers, grouped in areas that mostly matched CQRS folder.
2. The `CQRS` folder with command, query, and event handlers, but handlers were put in the
   `Processes` folder inside of the area,
3. The `CQRS` folder with command, query, and event handlers, grouped in areas.

All have a number of drawbacks:

1. The `CQRS` folder name is artificial.
2. Event handlers are sometimes not directly related to aggregates, but the areas in the `CQRS`
   folder follow aggregate patterns.
3. If there is a process that starts in a command and then follows several handlers, starting in
   the `CQRS` and then switching to the `Processes` results in quite far jumps in the code. This is
   a quite common case.

Additionally, there are some discrepancies where _events_ are located. Some projects have
a top-level `Events` folder in the domain, others have a per-aggregate (per-area) `Events` folder.

## Decision

We decided to go with a modified (3) approach, namely:

1. The `CQRS` folder will be renamed to `Handlers`,
2. All handlers will be put nearby, divided into areas,
3. If an event handler is not directly related to the aggregate, it should be put in the
   best-matched folder.

Moreover, all events should be put near the aggregate that raises the event, namely either

1. As a sibling to the aggregate (if there are few events),
2. In the `Events` folder, in the same area as the aggregate.
