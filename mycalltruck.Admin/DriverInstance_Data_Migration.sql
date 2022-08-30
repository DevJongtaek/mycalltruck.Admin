-- 자사차 등록
INSERT INTO DriverInstances (DriverId, ClientId, SubClientId, ClientUserId, CarGubun, RequestFrom, RequestTo)
SELECT Drivers.DriverId, CandidateId AS ClientId, ISNULL(Drivers.SubClientId, 0) as SubClientId, ISNULL(Drivers.ClientUserId, 0) as ClientUserId 
	, ISNULL(Drivers.CarGubun, 2) as CarGubun, Drivers.RequestFrom, Drivers.RequestTo
FROM Drivers
LEFT JOIN DriverInstances ON Drivers.DriverId = DriverInstances.DriverId AND Drivers.CandidateId = DriverInstances.ClientId
WHERE DriverInstances.DriverInstanceId IS NULL

-- 타사차 등록
INSERT INTO DriverInstances (DriverId, ClientId, SubClientId, ClientUserId, GroupName, CarGubun, RequestFrom, RequestTo)
SELECT DriverGroups.DriverId, DriverGroups.ClientId, ISNULL(DriverGroups.SubClientId, 0) as SubClientId, ISNULL(DriverGroups.ClientUserId, 0) as ClientUserId 
	, DriverGroups.Name as GroupName, CASE WHEN DriverGroups.Cont = 1 THEN 3 ELSE 4 END   AS CarGubun, Drivers.RequestFrom, Drivers.RequestTo 
FROM DriverGroups
JOIN Drivers ON DriverGroups.DriverId = Drivers.DriverId
LEFT JOIN DriverInstances ON DriverGroups.DriverId = DriverInstances.DriverId AND DriverGroups.ClientId = DriverInstances.ClientId AND ISNULL(DriverGroups.SubClientId, 0) = DriverInstances.SubClientId
AND ISNULL(DriverGroups.ClientUserId, 0) = DriverInstances.ClientUserId
WHERE DriverGroups.ClientId != Drivers.CandidateId AND DriverInstances.DriverInstanceId IS NULL


-- 자사차 그룹 설정
UPDATE DriverInstances
SET GroupName = DriverGroups.Name
FROM DriverInstances
JOIN DriverGroups ON DriverGroups.DriverId = DriverInstances.DriverId AND DriverGroups.ClientId = DriverInstances.ClientId AND ISNULL(DriverGroups.SubClientId, 0) = DriverInstances.SubClientId
AND ISNULL(DriverGroups.ClientUserId, 0) = DriverInstances.ClientUserId
JOIN Drivers ON Drivers.DriverId = DriverInstances.DriverId AND Drivers.CandidateId = DriverInstances.ClientId
WHERE DriverGroups.Name IN ('A', 'B', 'C')