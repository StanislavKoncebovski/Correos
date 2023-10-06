# Correos

Simple lightweight framework in C# for the loose implementation of the request pattern.

## Motivation

The major motivation to create Correos, a simple and lightweight framework for the loose correspondence between components of C# programs was to enable a simple way to realize the request-response pattern.

In my opinion, standard solutions based either upon services or otherwise are unncessarily complicated for typical use cases and can be replaced through a rather simple and straightforward mechanism implementing a central dispatching instance. 

*Correos* is an abbreviation of the original **Co**mmand, **Re**quest, and **E**vent **S**erver, it means *Post*, *Post Oﬃce* in Spanish and reflects the concept of communication. I am aware of the fact that the idea itself may be an old one and part of classical programmers' folklore; the code presented here is my independent implementation.

## Scope

*Correos* contains classes and routines for the synchronous and asynchronous realization of the request / response and notification patterns.


