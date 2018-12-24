﻿PRAGMA temp_store = MEMORY;

CREATE TEMP TABLE _Result (
    [CountTrackHistories]       INTEGER
   ,[CountTrackHistoryStates]   INTEGER
   ,[EarliestHistoryUtc]        DATETIME
   ,[LatestHistoryUtc]          DATETIME
);

DELETE FROM [TrackHistoryState]
WHERE  [TrackHistoryID] = @TrackHistoryID;

INSERT INTO _Result (
    [CountTrackHistoryStates]
) VALUES (
    CHANGES()
);

UPDATE _Result
SET    [EarliestHistoryUtc] = (SELECT [CreatedUtc] FROM [TrackHistory] WHERE  [TrackHistoryID] = @TrackHistoryID);

UPDATE _Result
SET    [LatestHistoryUtc] = [EarliestHistoryUtc];

DELETE FROM [TrackHistory]
WHERE  [TrackHistoryID] = @TrackHistoryID;

UPDATE _Result
SET    [CountTrackHistories] = CHANGES();

SELECT *
FROM   _Result;
