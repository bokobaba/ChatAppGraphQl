namespace ChatAppGraphQl.Queries.PostQueries {
    public record EditPostInput(
        Guid id,
        string Title,
        string Text
    ){
    }
}
