# Contributing Guideline

## You can contribute in many ways

Use the library and give feedback: report bugs, request features.
Fix bugs and issues.
Add new features for Databricks REST APIs that haven't been supported by this library.
Review pull requests from other contributors.

## How to contribute?

You can give feedback, report bugs and request new features anytime by opening an issue. Also, you can up-vote or comment on existing issues.

If you want to add code, examples or documentation to the repository, follow this process:

### Propose a contribution

- Open an issue, or comment on an existing issue to discuss your contribution and design, to ensure your contribution is a good fit and doesn't duplicate on-going work.
- All contributions need to comply with the MIT License. Contributors external to Microsoft need to sign CLA.

### Implement your contribution

1. Fork the repository.
2. Implement the contribution in your fork.
3. Create unit tests to test the serialization and deserialization logic.
4. Write sample code to test the contribution against a real Databricks REST API server. In many cases, we noticed the Databricks REST API documentation differs from the real API server's behavior, so it is important to test the implementation in real environment.
5. Run `dotnet format whitespace` from the `csharp` folder to auto-format your code.

### Open a pull request

- Open a pull request, and link it to the discussion issue you created earlier.
- Fix any build failures.
- Wait for code reviews from team members and others.
- Fix issues found in code review and re-iterate.

### Build and check-in

- Wait for a team member to merge your code in.

If in doubt about how to do something, see how it was done in existing code or pull requests, and don't hesitate to ask.
