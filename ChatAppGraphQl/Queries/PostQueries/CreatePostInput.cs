namespace ChatAppGraphQl.Queries.PostQueries {
    public record CreatePostInput(
        string CommunityName,
        string Title,
        string Text
    ){}
}
