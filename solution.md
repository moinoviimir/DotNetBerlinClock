# Technical documentation

This document describes the assumptions, alongisde their corresponding rationales, behind the implementation of the solution. It does not directly correspond to a commonly understood basic technical documentation, as the problem space is small enough and the task is reasonably specific, thus it is rather ambitious in terms of scope. 

## Design rationale

### Rationale

Despite the name, we are not building a clock here, because the specified interface does not have state. Clocks are essentially stateful, with a single piece of behaviour, the ticking; the interface we're implementing, however, as seen in the tests, is purely a presentation detail: we expect the ticking mechanism, with all of its timezone- and leap-interval-related complexities, to reside elsewhere, and the `ITimeConverter` interface implementation is merely an interpreting mechanism. If we were to imagine a real-world application, then the closest analogy - albeit quite a bit of a stretch - would be an intermediary between a ticking mechanism, and a set of physical lamps governed by a microcontroller; under this comparison, our component would interpret time in a way that would be essentially easy to render based off of string parsing. To be very frank, I'm afraid I do not see the separation between interpreting and rendering as viable here, because the two are tightly coupled by domain logic - the only way to use this presentation is to render it in a series of Berlin Clock lamps - but this is a hypothetical exercise, and as such, we'll have to approach it as if there was a use-case, and implement it as such.

Because we find ourselves in need of a synthetic use-case, I've taken the liberty of making the following assumption: the lamps used by our clock can have multiple colours, which may change depending on weather or time-of-year to improve visibility. This creates enough incentive for us to introduce composability into the project instead of writing a small set of bare-bones static methods that would appropriately correspond to the tasks's initial complexity.

### Design

Because we're building an interpreting mechanism, this is essentially an example of the `Adapter` pattern: our single interface method should be a pure function, essentially memoizable if one were so inclined. However, because of the use-case we've introduced, we'll have two components inside the solution: one inteprets the timestamp and maps in to an internal representation, and the other maps that representation to the final string-based output using an injected translation table.

## Implementation

### Technical architecture

Quality attributes obviously depend heavily on usage; because we have no technical or business stakeholders to talk to, I'll take creative liberty to say that they want Maintainability and Testability as primary attributes. One could add that a Performance quality attribute would also be important, but common sense dictates that, because we only render, and a person checking an ourside clock is not concerned with granularity higher than a second, a quality metric here would be that the solution runs faster than once every second, which, to us, means that there's zero need to worry about optimization or memory management and what not; the quality metric is loose enough that we may have as many GC runs as needed and still be fine.

### Consequences of TA

- There will be two component boundaries within the application, and all unit tests will run against those boundaries: the `TimeParser` component, and the `ColourMapper` component.
- I will not use `Span<T>` et al in the solution, in the interest of maintainability.
- Although we deal with the date-and-time domain here, no part of our behaviour is in fact in need of time-specific knowledge; as such, `NodaTime` is not required to manage time information.
