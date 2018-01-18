/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../typings/google.maps.d.ts" />
/// <reference path="../Scripts/typings/flot/jquery.flot.d.ts" />
/// <reference path="site.ts" />
/// <reference path="StravaCommon.ts" />
/// <reference path="ElevationCalculator.ts" />
/// <reference path="PrintMapControl.ts"/>
var marker;
var elevationCalc;
var directionsDisplay;
var map;
var chartData = [];
var selectionPolyline;
var points = [];
var markers = [];
var locations = [];
var elevations = [];
var locationsAdded = 1;
var calculatingRoute = false;
var plot;
function hasUrl() {
    return (getParameterByName("url") != null);
}
function setTitleFromDocument(data) {
    // title of page should be the name of the GPX file
    var names = data.getElementsByTagName("name");
    if (names.length > 0) {
        var name_1 = $(names[0]).text();
        document.title = name_1;
        $("h1").text(name_1);
    }
}
function addRouteLine(locations) {
    var polyline = new google.maps.Polyline({
        map: map,
        path: locations,
        strokeColor: "#0000FF",
        strokeOpacity: 1.0,
        strokeWeight: 5
    });
    addDistanceMarkers(map, locations);
}
function loadGpx(data) {
    setTitleFromDocument(data);
    var bounds = new google.maps.LatLngBounds();
    var gpxPoints = data.getElementsByTagName("trkpt");
    if (gpxPoints.length === 0) {
        // if this is a route, then route the waypoints
        gpxPoints = data.getElementsByTagName("rtept");
        for (var j = 0; j < gpxPoints.length; j++) {
            var latLng = addPointAndExtendBounds(gpxPoints[j], bounds);
            getRouteLocationInfo(latLng, "");
        }
        map.fitBounds(bounds);
        calcRouteOrGetElev();
        return;
    }
    var elevPoints = data.getElementsByTagName("ele");
    var containsElevation = gpxPoints.length === elevPoints.length;
    // load up locations and elevations and display
    for (var i_1 = 0; i_1 < gpxPoints.length; i_1++) {
        var latLng2 = addPointAndExtendBounds(gpxPoints[i_1], bounds);
        locations.push(latLng2);
        if (containsElevation) {
            elevations.push(parseFloat($(gpxPoints[i_1]).text()));
        }
    }
    // add line
    addRouteLine(locations);
    // zoom to fit
    map.fitBounds(bounds);
    if (containsElevation) {
        showResults(elevations);
    }
    else {
        elevationCalc = new ElevationCalculator(locations, function (message) { updateStatus(message); }, $("#elevGoogle").is(":checked"), function (elevations) { showResults(elevations); });
        elevationCalc.calculate();
    }
}
function getNodeText(node) {
    if (node == null) {
        return "";
    }
    var value = node.text;
    if (node.textContent) {
        value = node.textContent;
    }
    return value;
}
function loadRouteKml(data) {
    setTitleFromDocument(data);
    var kmlPoints = data.getElementsByTagName("LineString");
    if (kmlPoints.length < 1) {
        updateStatus("KML file does not contain a route");
        return;
    }
    else if (kmlPoints.length > 1) {
        updateStatus("KML file contains more than one route");
        return;
    }
    var bounds = new google.maps.LatLngBounds();
    var route = kmlPoints[0];
    var pts = route.getElementsByTagName("coordinates")[0];
    var ptsText = getNodeText(pts);
    var splitPoints = ptsText.split("\n");
    for (var i_2 = 0; i_2 < splitPoints.length; i_2++) {
        var splitPoint = splitPoints[i_2].split(",");
        var lng = parseFloat(splitPoint[0]);
        var lat = parseFloat(splitPoint[1]);
        if (!isNaN(lng) && !isNaN(lat)) {
            var latLng = new google.maps.LatLng(lat, lng);
            bounds.extend(latLng);
            locations.push(latLng);
        }
    }
    // add line
    addRouteLine(locations);
    // zoom to fit
    map.fitBounds(bounds);
    // calculate elvation
    elevationCalc = new ElevationCalculator(locations, function (message) { updateStatus(message); }, $("#elevGoogle").is(":checked"), function (elevations) { showResults(elevations); });
    elevationCalc.calculate();
}
function loadUrl() {
    // if we have url query string, load up the GPX
    if (hasUrl()) {
        updateStatus("Loading data...");
        var url_1 = getParameterByName("url");
        $.ajax({
            url: url_1,
            dataType: "xml",
            success: function (data) {
                if (data == null) {
                    updateStatus("Failed to load");
                    return;
                }
                if (url_1.toLowerCase().indexOf(".gpx") > -1) {
                    loadGpx(data);
                }
                else {
                    loadRouteKml(data);
                }
                updateStatus("");
            },
            error: function () {
                updateStatus("Failed to load");
            }
        });
    }
}
$("#elevationOf").change(function () {
    if ($("#elevationOf").val() === "Route") {
        $(".route").show();
    }
    else {
        $(".route").hide();
    }
});
function addPointAndExtendBounds(pt, bounds) {
    var latLng = gpxPointToLatLng(pt);
    bounds.extend(latLng);
    return latLng;
}
function gpxPointToLatLng(pt) {
    return new google.maps.LatLng(parseFloat(pt.getAttribute("lat")), parseFloat(pt.getAttribute("lon")));
}
function updateStatus(status) {
    $("#info").html(status);
}
function clearMarker() {
    if (marker != null) {
        marker.setMap(null);
    }
}
function clearFields() {
    // cancel any current operation
    if (elevationCalc != null) {
        elevationCalc.stopCalculation();
        elevationCalc = null;
    }
    points = [];
    buildRoutePoints();
    for (var i_3 = 0; i_3 < markers.length; i_3++) {
        markers[i_3].setMap(null);
    }
    markers = [];
    clearResultsFields();
}
function clearResultsFields() {
    // clear all fields
    $("#info").html("");
    clearMyFields();
    clearMarker();
    if (selectionPolyline) {
        selectionPolyline.setMap(null);
    }
    directionsDisplay.setMap(null);
    directionsDisplay.setPanel(null);
    $("#elevChart").height(0);
}
function clearMyFields() {
    $("#selectionInfo").html("");
    $("#distance").html("");
    $("#start").html("");
    $("#end").html("");
    $("#min").html("");
    $("#max").html("");
    $("#ascent").html("");
    $("#descent").html("");
    $("#elevChart").html("");
}
$(document).ready(function () {
    loadGoogleMaps("places,geometry", function () {
        var latlng = new google.maps.LatLng(54.559322, -4.174804);
        var options = {
            zoom: 6,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            fullscreenControl: true,
            gestureHandling: getGestureHandling()
        };
        map = new google.maps.Map(document.getElementById("map"), options);
        map.controls[google.maps.ControlPosition.TOP_RIGHT].push(printMapControl(map));
        // click handler
        if (!hasUrl()) {
            $("#tabs, #edit-buttons, #description, #output-tabs").show();
            map.setOptions({ draggableCursor: "crosshair" });
            google.maps.event.addListener(map, "click", function (location) {
                getRouteLocationInfo(location.latLng, "Location " + locationsAdded);
                locationsAdded++;
            });
        }
        // set up directions renderer
        var rendererOptions = {
            draggable: true,
            markerOptions: { visible: false }
        };
        directionsDisplay = new google.maps.DirectionsRenderer(rendererOptions);
        google.maps.event.addListener(directionsDisplay, "directions_changed", function () {
            if (!calculatingRoute) {
                updateStatus("Route changed");
                if (elevationCalc != null) {
                    elevationCalc.stopCalculation();
                    elevationCalc = null;
                }
                getElevationOfRoute(directionsDisplay.getDirections().routes[0]);
            }
        });
        google.maps.event.addListener(directionsDisplay, "routeindex_changed", function () {
            if (!calculatingRoute) {
                updateStatus("Route index changed");
                if (elevationCalc != null) {
                    elevationCalc.stopCalculation();
                    elevationCalc = null;
                }
                getElevationOfRoute(directionsDisplay.getDirections().routes[directionsDisplay.getRouteIndex()]);
            }
        });
        // autocomplete
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById("location"), { bounds: null, componentRestrictions: null, types: [] });
        google.maps.event.addListener(autocomplete, "place_changed", function () {
            var place = autocomplete.getPlace();
            if (place != null && place.geometry != null) {
                getRouteLocationInfo(place.geometry.location, $("#location").val());
                map.setCenter(place.geometry.location);
            }
            $("#location").val("");
        });
        // load the file if required
        loadUrl();
    });
});
function routeAddLatLng() {
    var latLong = new google.maps.LatLng($("#lat").val(), $("#lng").val());
    getRouteLocationInfo(latLong, "Location " + locationsAdded);
    locationsAdded++;
    map.setCenter(latLong);
    $("#lat").val("");
    $("#lng").val("");
}
function addRouteLocation() {
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ address: $("#location").val() }, function (results) {
        if (results && results[0]) {
            var result = results[0];
            var latLong_1 = result.geometry.location;
            getRouteLocationInfo(latLong_1, $("#location").val());
            map.setCenter(latLong_1);
            $("#location").val("");
        }
        else {
            showError("Location not found");
        }
    });
}
function getRouteLocationInfo(latlng, locationName) {
    if (latlng != null) {
        var point = { latLng: latlng, locationName: locationName };
        points.push(point);
        buildRoutePoints();
    }
}
function clearRouteMarkers() {
    for (var i_4 = 0; i_4 < markers.length; i_4++) {
        markers[i_4].setMap(null);
    }
    markers = [];
}
function buildRoutePoints() {
    clearRouteMarkers();
    var html = "";
    for (var i_5 = 0; i_5 < points.length; i_5++) {
        var marker_1 = new google.maps.Marker({
            position: points[i_5].latLng,
            title: points[i_5].locationName,
            icon: "https://www.doogal.co.uk/images/red.png"
        });
        markers.push(marker_1);
        marker_1.setMap(map);
        html += "<tr><td>" + points[i_5].locationName + "</td><td>" + roundNumber(points[i_5].latLng.lat(), 6) +
            "</td><td>" + roundNumber(points[i_5].latLng.lng(), 6) +
            "</td><td><button class=\"delete btn\" onclick=\"routeRemoveRow(" + i_5 + ");\">Delete</button></td><td>";
        if (i_5 < points.length - 1) {
            html += "<button class=\"moveDown btn\" onclick=\"routeMoveRowDown(" + i_5 + ");\">Move down</button>";
        }
        html += "</td><td>";
        if (i_5 > 0) {
            html += "<button class=\"moveUp btn\" onclick=\"routeMoveRowUp(" + i_5 + ");\">Move up</button>";
        }
        html += "</td></tr>";
    }
    $("#waypointsLocations tbody").html(html);
}
function calcRouteOrGetElev() {
    if (elevationCalc != null) {
        showError("I'm busy right now! Wait for the last operation to complete");
        return;
    }
    if (points.length === 0) {
        updateStatus("No locations added!");
        return;
    }
    // if straight line then just get the elevation
    if ($("#elevationOf").val() === "Route") {
        calculateRoute();
    }
    else {
        getElevationOfLine();
    }
}
function calculateRoute() {
    // calculate the route
    updateStatus("Calculating route...");
    var directions = new google.maps.DirectionsService();
    var routeType = $("#routeType").val();
    var travelMode = google.maps.TravelMode.DRIVING;
    if (routeType === "Walking") {
        travelMode = google.maps.TravelMode.WALKING;
    }
    else if (routeType === "Public transport") {
        travelMode = google.maps.TravelMode.TRANSIT;
    }
    else if (routeType === "Cycling") {
        travelMode = google.maps.TravelMode.BICYCLING;
    }
    var directionUnits = google.maps.UnitSystem.METRIC;
    if (showImperial()) {
        directionUnits = google.maps.UnitSystem.IMPERIAL;
    }
    var waypts = [];
    var end = points.length - 1;
    var dest = points[end].latLng;
    if ($("#roundTrip").is(":checked")) {
        end = points.length;
        dest = points[0].latLng;
    }
    for (var i_6 = 1; i_6 < end; i_6++) {
        waypts.push({ location: points[i_6].latLng });
    }
    var request = {
        origin: points[0].latLng,
        destination: dest,
        waypoints: waypts,
        travelMode: travelMode,
        unitSystem: directionUnits,
        provideRouteAlternatives: true
    };
    if (travelMode === google.maps.TravelMode.TRANSIT) {
        request.transitOptions = { departureTime: new Date() };
    }
    if ($("#avoidTolls").is(":checked")) {
        request.avoidTolls = true;
    }
    if ($("#avoidHighways").is(":checked")) {
        request.avoidHighways = true;
    }
    directionsDisplay.setMap(map);
    directionsDisplay.setPanel(document.getElementById("directions"));
    calculatingRoute = true;
    directions.route(request, function (result, status) {
        if (status === google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(result);
            updateStatus("Route calculated");
            calculatingRoute = false;
            getElevationOfRoute(result.routes[0]);
        }
        else {
            var statusText = getDirectionStatusText(status);
            updateStatus("An error occurred calculating the route - " + statusText);
            calculatingRoute = false;
        }
    });
}
function getElevationOfRoute(route) {
    clearMyFields();
    clearMarker();
    // show distance
    var totalDistance = 0;
    var time = 0;
    for (var x = 0; x < route.legs.length; x++) {
        var theLeg = route.legs[x];
        totalDistance += theLeg.distance.value;
        time += theLeg.duration.value;
    }
    $("#distance").html(showRouteDistance(totalDistance));
    // get all the lat/longs
    locations = [];
    for (var i_7 = 0; i_7 < route.legs.length; i_7++) {
        var thisLeg = route.legs[i_7];
        for (var j = 0; j < thisLeg.steps.length; j++) {
            var thisStep = thisLeg.steps[j];
            for (var k = 0; k < thisStep.path.length; k++) {
                // ignore first location in each step except for the very first location
                if (k > 0 || (i_7 === 0 && j === 0)) {
                    if (k > 0) {
                        // add extra points if distance is too great
                        var distance_1 = google.maps.geometry.spherical
                            .computeDistanceBetween(thisStep.path[k], thisStep.path[k - 1]);
                        if (distance_1 > 100) {
                            var count_1 = Math.ceil(distance_1 / 100);
                            var latDiff = thisStep.path[k].lat() - thisStep.path[k - 1].lat();
                            var lngDiff = thisStep.path[k].lng() - thisStep.path[k - 1].lng();
                            for (var l = 1; l < count_1; l++) {
                                var newLat = thisStep.path[k - 1].lat() + ((latDiff / count_1) * l);
                                var newLng = thisStep.path[k - 1].lng() + ((lngDiff / count_1) * l);
                                locations.push(new google.maps.LatLng(newLat, newLng));
                            }
                        }
                    }
                    locations.push(thisStep.path[k]);
                }
            }
        }
    }
    addDistanceMarkers(map, locations);
    elevationCalc = new ElevationCalculator(locations, function (message) { updateStatus(message); }, $("#elevGoogle").is(":checked"), function (elevations) { showResults(elevations); });
    elevationCalc.calculate();
}
function getElevationOfLine() {
    updateStatus("Calculating elevation...");
    var path = [points[0].latLng, points[points.length - 1].latLng];
    var pathRequest = {
        path: path,
        samples: 256
    };
    var bounds = new google.maps.LatLngBounds();
    bounds.extend(points[0].latLng);
    bounds.extend(points[points.length - 1].latLng);
    map.fitBounds(bounds);
    var elevator = new google.maps.ElevationService();
    elevator.getElevationAlongPath(pathRequest, function (results, status) {
        if (status === google.maps.ElevationStatus.OK) {
            var elevations_1 = [];
            locations = [];
            for (var i_8 = 0; i_8 < results.length; i_8++) {
                elevations_1.push(results[i_8].elevation);
                locations.push(results[i_8].location);
            }
            showResults(elevations_1);
            updateStatus("Elevation calculated using " + results.length + " samples");
        }
        else {
            updateStatus("An error occurred calculating the elevation - " +
                ElevationCalculator.elevationStatusDescription(status));
        }
    });
}
function smooth(rawValues) {
    var dataLength = rawValues.length;
    var smoothingSize = 5;
    var toSmooth = rawValues;
    var newElevations = [];
    for (var i_9 = 0; i_9 < dataLength; i_9++) {
        var sumValues = 0;
        var start = i_9 - smoothingSize;
        if (start < 0) {
            start = 0;
        }
        var end = i_9 + smoothingSize;
        if (end > dataLength - 1) {
            end = dataLength - 1;
        }
        for (var j = start; j <= end; j++) {
            sumValues += toSmooth[j];
        }
        newElevations.push(sumValues / (end - start + 1));
    }
    return newElevations;
}
function showResults(returnedElevations) {
    if (returnedElevations == null) {
        return;
    }
    updateStatus("Building output data...");
    elevations = returnedElevations;
    elevationCalc = null;
    // smooth the data
    if ($("#smoothElevation").is(":checked")) {
        elevations = smooth(elevations);
    }
    // display the results
    chartData = [];
    var ascent = getClimbTotalAscent(elevations);
    var descent = 0;
    showRouteElevation("#start", elevations[0], showImperial());
    showRouteElevation("#end", elevations[elevations.length - 1], showImperial());
    var minElevation = elevations[0];
    var maxElevation = elevations[0];
    var distance = 0;
    var maxGradient = 0;
    var maxGradientAt = 0;
    var maxGradientDesc = 0;
    var maxGradientDescAt = 0;
    for (var i_10 = 0; i_10 < elevations.length; i_10++) {
        minElevation = Math.min(elevations[i_10], minElevation);
        maxElevation = Math.max(elevations[i_10], maxElevation);
        // calculate distance
        if (i_10 > 0) {
            var d = google.maps.geometry.spherical.computeDistanceBetween(locations[i_10 - 1], locations[i_10]);
            distance += d;
        }
        var elevation = elevations[i_10];
        if (showImperial()) {
            elevation = metresToFeet(elevations[i_10]);
        }
        var displayDistance = distance / 1000;
        if (showImperial()) {
            displayDistance = kmToMiles(distance / 1000);
        }
        chartData.push([displayDistance, elevation]);
        if (i_10 > 0) {
            var thisAscent = elevations[i_10] - elevations[i_10 - 1];
            if (thisAscent < 0) {
                descent -= thisAscent;
            }
        }
        // calculate max gradient
        if (showGradient(i_10)) {
            var thisGradient = calculateRouteGradient(i_10);
            if (thisGradient > maxGradient) {
                maxGradient = thisGradient;
                maxGradientAt = distance;
            }
            if (thisGradient < maxGradientDesc) {
                maxGradientDesc = thisGradient;
                maxGradientDescAt = distance;
            }
        }
    }
    if (maxGradient > 0) {
        $("#maxGradient").html(maxGradient + "% at " + showRouteDistance(maxGradientAt));
    }
    else {
        $("#maxGradient").html("N/A");
    }
    if (maxGradientDesc < 0) {
        $("#maxDescGradient").html(Math.abs(maxGradientDesc) + "% at " + showRouteDistance(maxGradientDescAt));
    }
    else {
        $("#maxDescGradient").html("N/A");
    }
    $("#distance").html(showRouteDistance(distance));
    showRouteElevation("#ascent", ascent, showImperial());
    showRouteElevation("#descent", descent, showImperial());
    showRouteElevation("#min", minElevation, showImperial());
    showRouteElevation("#max", maxElevation, showImperial());
    var elevChange = maxElevation - minElevation;
    // chart
    if (elevChange < 50) {
        $("#elevChart").height(100);
    }
    else if (elevChange >= 300) {
        $("#elevChart").height(600);
    }
    else {
        $("#elevChart").height(elevChange * 2);
    }
    updateStatus("Plotting chart...");
    var chart = $("#elevChart");
    var chartMin = elevationChartMin(minElevation, maxElevation);
    var chartMax = elevationChartMax(minElevation, maxElevation);
    if (showImperial()) {
        chartMin = metresToFeet(chartMin);
        chartMax = metresToFeet(chartMax);
    }
    plot = $.plot(chart, [{ data: chartData, lines: { show: true, fill: true, fillColor: "#D9D9D9" } }], {
        grid: {
            hoverable: true,
            aboveData: true
        },
        yaxis: {
            min: chartMin,
            max: chartMax
        },
        crosshair: {
            mode: "x"
        },
        selection: {
            mode: "x",
            color: "red"
        },
        colors: ["#D9D9D9"]
    });
    $("#selectionInfo").html("Select part of the chart to grade a climb<hr/>");
    chart.on("mouseleave", function () {
        $("#tooltip").hide();
    });
    chart.bind("plotunselected", function () {
        $("#selectionInfo").html("");
        if (selectionPolyline != null) {
            selectionPolyline.setMap(null);
        }
    });
    chart.bind("plotselected", function (event, ranges) {
        var from = ranges.xaxis.from;
        var to = ranges.xaxis.to;
        // get the start and end altitudes
        var fromIndex = getIndexForDistance(from);
        var fromElevation = chartData[fromIndex][1];
        var toIndex = getIndexForDistance(to);
        var toElevation = chartData[toIndex][1];
        var segmentDistance = to - from;
        // selected average %age
        var segmentDistanceInMetres = segmentDistance * 1000;
        var climbInMetres = Math.abs(toElevation - fromElevation);
        // cope with imperial
        if (showImperial()) {
            segmentDistanceInMetres = milesToKMs(segmentDistance) * 1000;
            climbInMetres = feetToMetres(toElevation - fromElevation);
        }
        var climbCategory = getClimbCategory(climbInMetres, segmentDistanceInMetres);
        var grade = getClimbGrade(climbInMetres, segmentDistanceInMetres);
        var score = getClimbScore(climbInMetres, segmentDistanceInMetres);
        var selectionHtml = "<table>" + "<tr><td>Segment selected: </td><td>" +
            from.toFixed(1) + distanceUnitName(showImperial()) +
            " to " + to.toFixed(1) + distanceUnitName(showImperial()) + "</td></tr>" +
            "<tr><td>Distance selected: </td><td>" +
            segmentDistance.toFixed(1) + distanceUnitName(showImperial()) + "</td></tr>" +
            "<tr><td>Selected start elevation: </td><td>" +
            roundNumber(fromElevation, 0) + elevationUnits(showImperial()) + "</td></tr>" +
            "<tr><td>Select end elevation: </td><td>" +
            roundNumber(toElevation, 0) + elevationUnits(showImperial()) + "</td></tr>" + "<tr><td>Average grade: </td><td>" +
            roundNumber(grade, 1) + "%</td></tr>" + "<tr><td>Climb score: </td><td>" + roundNumber(score, 0) +
            "</td></tr>" + "<tr><td>Climb category: </td><td>" + climbCategory + "</td></tr>" + "</table><hr/>";
        $("#selectionInfo").html(selectionHtml);
        // show the segment on the map
        if (selectionPolyline != null) {
            selectionPolyline.setMap(null);
        }
        var sectionData = locations.slice(fromIndex, toIndex);
        var polyOptions = {
            map: map,
            strokeColor: "red",
            strokeWeight: 10,
            path: sectionData,
            zIndex: 1000
        };
        selectionPolyline = new google.maps.Polyline(polyOptions);
    });
    chart.bind("plothover", function (event, pos) {
        var dataset = plot.getData();
        var series = dataset[0];
        var j = getIndexForDistance(pos.x);
        $("#tooltip").hide();
        var y = series.data[j][1];
        var x = series.data[j][0];
        var name = "Elevation: " +
            addCommas(roundNumber(y, 0)) +
            elevationUnits(showImperial()) +
            "<br/>" +
            "Distance: " +
            addCommas(roundNumber(x, 1)) +
            distanceUnitName(showImperial());
        if (showGradient(j)) {
            name += "<br/>" + "Gradient: " + calculateRouteGradient(j) + "%";
        }
        showTooltip(pos.pageX, pos.pageY, name);
        // add marker
        if (marker == null || marker.getMap() == null) {
            marker = new google.maps.Marker({
                position: locations[j],
                map: map,
                icon: {
                    url: "images/station6.png",
                    anchor: new google.maps.Point(10, 10)
                },
                title: "Elevation: " +
                    addCommas(roundNumber(y, 0)) +
                    elevationUnits(showImperial()) +
                    ", distance: " +
                    addCommas(roundNumber(x, 1)) +
                    distanceUnitName(showImperial())
            });
        }
        else {
            marker.setPosition(locations[j]);
        }
    });
    updateStatus("Complete");
}
function showGradient(j) {
    return j > 5;
}
function calculateRouteGradient(j) {
    var totalDistance = 0;
    var i = j;
    while (totalDistance < 100 && i > 0) {
        var distance_2 = google.maps.geometry.spherical.computeDistanceBetween(locations[i], locations[i - 1]);
        totalDistance += distance_2;
        i--;
    }
    var elevationDiff = elevations[j] - elevations[i];
    return Math.round(100 * elevationDiff / totalDistance);
}
function getIndexForDistance(distance) {
    for (var i_11 = 0; i_11 < chartData.length; i_11++) {
        if (chartData[i_11][0] >= distance) {
            return i_11;
        }
    }
    return chartData.length - 1;
}
function showImperial() {
    return $("#directionUnits").val() === "Miles";
}
function showRouteElevation(selector, elevationM, showImperial) {
    if (showImperial) {
        $(selector).html(Math.round(metresToFeet(elevationM)) + " feet");
    }
    else {
        $(selector).html(Math.round(elevationM) + " metres");
    }
}
function showRouteDistance(distanceM) {
    if (showImperial()) {
        return Math.round(kmToMiles(distanceM) / 100) / 10 + distanceUnitName(showImperial());
    }
    else {
        return Math.round(distanceM / 100) / 10 + distanceUnitName(showImperial());
    }
}
function downloadRouteGpx() {
    buildGpx();
    $("#gpxForm").submit();
}
function buildGpx() {
    // start GPX data
    var name = $("#waypointsLocations td:first").text() + " to " +
        $("#waypointsLocations tr:last td:first").text();
    var gpx = gpxStart(name);
    for (var i_12 = 0; i_12 < elevations.length; i_12++) {
        // update GPX
        gpx += gpxPoint(locations[i_12], elevations[i_12]);
    }
    gpx += gpxEnd();
    $("#gpxData").val(gpx);
}
function downloadKml() {
    // start KML
    var kmlCode = kmlDocumentStart("Route elevation") + kmlStyleThickLine() + "<Placemark>\n" + kmlLineStart();
    for (var i_13 = 0; i_13 < elevations.length; i_13++) {
        // update KML
        kmlCode += roundNumber(locations[i_13].lng(), 6) + "," + roundNumber(locations[i_13].lat(), 6) + " ";
    }
    // end the GPX and CSV and KML
    kmlCode += kmlLineEnd() + kmlStyleUrl("thickLine") + "</Placemark>\n" + kmlDocumentEnd();
    $("#kmlData").val(kmlCode);
    document.getElementById("kmlForm").submit();
}
function downloadCsv() {
    var csv;
    if (showImperial()) {
        csv = "Latitude,Longitude,Elevation (feet),Distance (miles),Gradient\n";
    }
    else {
        csv = "Latitude,Longitude,Elevation (metres),Distance (KMs),Gradient\n";
    }
    var distance = 0;
    for (var i_14 = 0; i_14 < elevations.length; i_14++) {
        // calculate distance
        if (i_14 > 0) {
            var d = google.maps.geometry.spherical.computeDistanceBetween(locations[i_14 - 1], locations[i_14]);
            distance += d;
        }
        var elevation = elevations[i_14];
        if (showImperial()) {
            elevation = metresToFeet(elevations[i_14]);
        }
        var displayDistance = distance / 1000;
        if (showImperial()) {
            displayDistance = kmToMiles(distance / 1000);
        }
        // update CSV
        csv += roundNumber(locations[i_14].lat(), 6) +
            "," +
            roundNumber(locations[i_14].lng(), 6) +
            "," +
            roundNumber(elevation, 1) +
            "," +
            roundNumber(displayDistance, 3) +
            ",";
        if (showGradient(i_14)) {
            csv += calculateRouteGradient(i_14);
        }
        csv += "\n";
    }
    $("#csvData").val(csv);
    document.getElementById("csvForm").submit();
}
function routeRemoveRow(index) {
    points.splice(index, 1);
    buildRoutePoints();
    clearResultsFields();
}
function routeMoveRowDown(index) {
    var item = points[index];
    points.splice(index, 1);
    points.splice(index + 1, 0, item);
    buildRoutePoints();
    clearResultsFields();
}
function routeMoveRowUp(index) {
    var item = points[index];
    points.splice(index, 1);
    points.splice(index - 1, 0, item);
    buildRoutePoints();
    clearResultsFields();
}
function saveRoute() {
    // save the route as GPX
    buildGpx();
    updateStatus("Uploading...");
    // post data to server
    $.ajax({
        url: "postGpx.ashx",
        type: "POST",
        data: { data: $("#gpxData").val() },
        success: function (data) {
            updateStatus("Complete. <a href=\"RouteElevation.php?url=GPX/" + data +
                "\">Save this link to view the route later</a>");
        },
        error: function () {
            updateStatus("Failed to upload route");
        }
    });
}
