# Technical documentation

This document describes the assumptions, alongisde their corresponding rationales, behind the implementation of the solution. The document addresses everything I could think of that's relevant for solution implementation, and thus it is rather ambitious in terms of scope.

I've written the document in the spirit of principles described in `Readme.md`.

## Design rationale

### Rationale

Despite the name, we are not building a clock here, because the specified interface does not have state. Clocks are essentially stateful, with a single piece of behaviour, the ticking; the interface we're implementing, however, as seen in the tests, addresses purely a presentation concern: we expect the ticking mechanism, with all of its timezone- and leap-interval-related complexities, to reside elsewhere, and the `ITimeConverter` interface implementation is merely an interpreting mechanism. If we were to imagine a real-world application, then the closest analogy - albeit with quite a bit of a stretch - would be an intermediary between a ticking mechanism, and a set of physical lamps governed by a microcontroller; under this comparison, our component would interpret time in a way that would be essentially easy to render based off of string parsing. To be very frank, I'm afraid I do not see the separation between interpreting and rendering as viable here, because the two are tightly coupled by domain logic - the only way to use this presentation is to render it in a series of Berlin Clock lamps, thus separation does not add value but adds maintenance costs - but this is a hypothetical exercise, and as such, we'll have to approach it as if there was a use-case, and implement it as such.

Because we find ourselves in need of a synthetic use-case, I've taken the liberty of making the following assumption: the lamps used by our clock can have multiple colours, which may change depending on weather or time-of-year to improve visibility. This creates enough incentive for us to introduce composability into the project instead of writing a small set of bare-bones static methods that would appropriately correspond to the tasks's initial complexity.

### Design

Because we're building an interpreting mechanism, this is essentially an example of the `Adapter` pattern: our single interface method should be a pure function, essentially memoizable if one were so inclined. However, because of the use-case we've introduced, we'll have two components inside the solution: one inteprets the timestamp and maps in to an internal representation, and the other maps that representation to the final string-based output using an injected translation table.

## Implementation

### Technical architecture

Quality attributes obviously depend heavily on usage; because we have no technical or business stakeholders to talk to, I'll take creative liberty to say that they want Maintainability and Testability as primary attributes. One could add that a Performance quality attribute would also be important, but common sense dictates that, because we only render, and since a person checking an outside clock is not concerned with granularity higher than seconds, a quality metric here would be that the solution runs faster than once every second, which, to us, means that there's zero need to worry about optimization or memory management and what not; the quality metric is loose enough that we may have as many GC runs as needed and still be fine.

### Consequences of TA

- There will be two component boundaries within the application, and all unit tests will run against those boundaries: the `TimeParser` component, and the `ColourMapper` component.
- I will not use `Span<T>` et al in the solution, in the interest of maintainability, because performance gains are irrelevant for us due to the established quality metric for Performance.
- Although we deal with the date-and-time domain here, no part of our behaviour is in fact in need of time-specific knowledge; as such, `NodaTime` is not required to manage time information.

## Quality

BDD is foundationally business-specific, but can be also be practiced to describe software component behaviour. As I'm not sure what exactly is practiced and it would be arrogant of me to assume, I'll write standard unit tests to verify the behaviour against component boundaries.

## Misc notes

I've taken the liberty of adjusting things where they didn't change the intent or behaviour of the project, such as swapping argument order in `Assert.AreEqual` (`expected` goes first, rather than the result, to avoid confusion with test runner output). As `MsTest1` has a difficult time working with parameterized tests, I've written them explicitly, with duplicated code.