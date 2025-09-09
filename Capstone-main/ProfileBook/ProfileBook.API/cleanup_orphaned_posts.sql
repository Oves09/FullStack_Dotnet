-- Clean up orphaned posts (posts whose users no longer exist)
-- Run this script to remove existing orphaned posts

-- First, let's see how many orphaned posts exist
SELECT COUNT(*) as OrphanedPostsCount
FROM Posts p
LEFT JOIN AspNetUsers u ON p.UserId = u.Id
WHERE u.Id IS NULL;

-- Delete orphaned posts
DELETE FROM Posts 
WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);

-- Verify cleanup
SELECT COUNT(*) as RemainingOrphanedPosts
FROM Posts p
LEFT JOIN AspNetUsers u ON p.UserId = u.Id
WHERE u.Id IS NULL;
