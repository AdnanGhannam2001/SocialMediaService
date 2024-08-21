using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaService.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Image = table.Column<bool>(type: "boolean", nullable: false),
                    CoverImage = table.Column<bool>(type: "boolean", nullable: false),
                    Visibility = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PhoneNumber_Value = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<byte>(type: "smallint", nullable: false),
                    Bio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Image = table.Column<bool>(type: "boolean", nullable: false),
                    CoverImage = table.Column<bool>(type: "boolean", nullable: false),
                    JobInformations_JobTitle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    JobInformations_Company = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Socials_Facebook = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    Socials_Youtube = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    Socials_Twitter = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupsSettings",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsSettings", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_GroupsSettings_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    BlockerId = table.Column<string>(type: "text", nullable: false),
                    BlockedId = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    BlockedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => new { x.BlockedId, x.BlockerId });
                    table.ForeignKey(
                        name: "FK_Blocks_Profiles_BlockedId",
                        column: x => x.BlockedId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blocks_Profiles_BlockerId",
                        column: x => x.BlockerId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    FollowerId = table.Column<string>(type: "text", nullable: false),
                    FollowedId = table.Column<string>(type: "text", nullable: false),
                    FollowedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => new { x.FollowedId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_Follows_Profiles_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follows_Profiles_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendshipRequests",
                columns: table => new
                {
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    ReceiverId = table.Column<string>(type: "text", nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendshipRequests", x => new { x.SenderId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_FriendshipRequests_Profiles_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendshipRequests_Profiles_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    FriendId = table.Column<string>(type: "text", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new { x.FriendId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_Friendships_Profiles_FriendId",
                        column: x => x.FriendId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendships_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SentAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => new { x.SenderId, x.ProfileId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_Invites_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invites_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invites_Profiles_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinRequests",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SentAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRequests", x => new { x.ProfileId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_JoinRequests_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinRequests_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KickedList",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    KickedById = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    KickedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KickedList", x => new { x.ProfileId, x.KickedById, x.GroupId });
                    table.ForeignKey(
                        name: "FK_KickedList_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KickedList_Profiles_KickedById",
                        column: x => x.KickedById,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KickedList_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    JointAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => new { x.ProfileId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_Members_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Visibility = table.Column<byte>(type: "smallint", nullable: false),
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SettingsList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<byte>(type: "smallint", nullable: false),
                    DateOfBirth = table.Column<byte>(type: "smallint", nullable: false),
                    Gender = table.Column<byte>(type: "smallint", nullable: false),
                    Phone = table.Column<byte>(type: "smallint", nullable: false),
                    JobTitle = table.Column<byte>(type: "smallint", nullable: false),
                    Company = table.Column<byte>(type: "smallint", nullable: false),
                    StartDate = table.Column<byte>(type: "smallint", nullable: false),
                    Socials = table.Column<byte>(type: "smallint", nullable: false),
                    Bio = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingsList_Profiles_Id",
                        column: x => x.Id,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    PostId = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HiddenPosts",
                columns: table => new
                {
                    HiddenById = table.Column<string>(type: "text", nullable: false),
                    HiddenId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiddenPosts", x => new { x.HiddenById, x.HiddenId });
                    table.ForeignKey(
                        name: "FK_HiddenPosts_Posts_HiddenId",
                        column: x => x.HiddenId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HiddenPosts_Profiles_HiddenById",
                        column: x => x.HiddenById,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostsMedia",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsMedia", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostsMedia_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    PostId = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    ReactedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => new { x.PostId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_Reactions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedPosts",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    PostId = table.Column<string>(type: "text", nullable: false),
                    SavedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedPosts", x => new { x.ProfileId, x.PostId });
                    table.ForeignKey(
                        name: "FK_SavedPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedPosts_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_BlockerId",
                table: "Blocks",
                column: "BlockerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProfileId",
                table: "Comments",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerId",
                table: "Follows",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipRequests_ReceiverId",
                table: "FriendshipRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_ProfileId",
                table: "Friendships",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_HiddenPosts_HiddenId",
                table: "HiddenPosts",
                column: "HiddenId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_GroupId",
                table: "Invites",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_ProfileId",
                table: "Invites",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_GroupId",
                table: "JoinRequests",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_KickedList_GroupId",
                table: "KickedList",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_KickedList_KickedById",
                table: "KickedList",
                column: "KickedById");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GroupId",
                table: "Members",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GroupId",
                table: "Posts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ProfileId",
                table: "Posts",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ProfileId",
                table: "Reactions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_PostId",
                table: "SavedPosts",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "FriendshipRequests");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "GroupsSettings");

            migrationBuilder.DropTable(
                name: "HiddenPosts");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropTable(
                name: "JoinRequests");

            migrationBuilder.DropTable(
                name: "KickedList");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "PostsMedia");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "SavedPosts");

            migrationBuilder.DropTable(
                name: "SettingsList");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
