namespace ChatAppGraphQl.Queries.CommunityQueries {
    public record CreateCommunityInput(
        string Name,
        byte[]? BannerImage = null
    ) {}
}
