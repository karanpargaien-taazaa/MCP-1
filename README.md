# GenAI + MCP: Generative AI Integration for Context-Aware Intelligence

## 🧠 The Idea
In the age of agents and AI-native experiences, users expect intelligent, voice-driven interfaces that go beyond static workflows. At Innago, we're building a voice-assist platform that can understand user intent and perform actions like creating leases, checking overdue invoices, or sending reminders — all from natural language input.
To support this, we needed a system that:
Understands natural language
Extracts relevant entities (like tenant name, property, unit)
Maps them to real database values
Uses that context to generate structured output
This is the goal of the GenAI + MCP project.

## 🔧 First Steps
We built a lightweight, extensible .NET library called GenAI that provides a type-safe abstraction over multiple language model providers (OpenAI, Google, etc.).

## ✨ GenAI Highlights
IGenAI interface for sending prompts
IGenAIResponse<T> for strong typed model output
Minimal, testable, pluggable design
public interface IGenAI
{
    Task<T> GetResponseAsync<T>(string prompt)
        where T : IGenAIResponse<T>, new();
}
public interface IGenAIResponse<T>
{
    T GetSampleInstance();
}

## 🔄 Context Awareness via MCP
Language models are unaware of your data. So, we built a runtime processor that:
Identifies user intent from input
Extracts entities (using GenAI)
Resolves them to real IDs (like propertyId, tenantId, etc.)
Sends final structured prompt to LLM for output

This allows for:
Structured lease creation
Invoice reminders
Other task automation

## Each intent has its own handler that works via:
- IEntityExtractor
- IContextResolver
- IPromptBuilder
- IGenAI

## 🧩 Next Step
We're extending the runtime to support multiple intents and improving it with OCP (Open-Closed Principle) for new workflows.

## 📂 Structure
```
GenAI/
├── IGenAI.cs
├── IGenAIResponse.cs
└── ...

MCP.Runtime/
├── IEntityExtractor.cs
├── IContextResolver.cs
├── IPromptBuilder.cs
├── IIntentHandler.cs
├── IIntentRouter.cs
└── Intent Implementations/
    ├── LeaseIntentHandler.cs
    ├── LeaseEntityExtractor.cs
    ├── LeaseContextResolver.cs
    ├── LeasePromptBuilder.cs
    └── ...
