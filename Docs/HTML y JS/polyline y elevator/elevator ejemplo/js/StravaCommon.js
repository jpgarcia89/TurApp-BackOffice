//#region Strava
// define our result classes
var SegmentMap = /** @class */ (function () {
    function SegmentMap() {
    }
    return SegmentMap;
}());
var SegmentElevation = /** @class */ (function () {
    function SegmentElevation() {
    }
    return SegmentElevation;
}());
var SegmentStats = /** @class */ (function () {
    function SegmentStats() {
    }
    return SegmentStats;
}());
var SegmentDetail = /** @class */ (function () {
    function SegmentDetail() {
    }
    return SegmentDetail;
}());
var StreetViewFlyby = /** @class */ (function () {
    function StreetViewFlyby(map, path) {
        this.svPosition = 0;
        this.svRunning = false;
        this.map = map;
        this.path = path;
    }
    StreetViewFlyby.prototype.running = function () {
        return this.svRunning;
    };
    StreetViewFlyby.prototype.run = function () {
        this.svRunning = true;
        this.streetViewFlybyNext();
    };
    StreetViewFlyby.prototype.pause = function () {
        this.svRunning = false;
    };
    StreetViewFlyby.prototype.streetViewFlybyNext = function () {
        var _this = this;
        if (this.svPosition >= this.path.length) {
            this.svPosition = 0;
        }
        var heading;
        if (this.svPosition < this.path.length - 1) {
            heading = google.maps.geometry.spherical.computeHeading(this.path[this.svPosition], this.path[this.svPosition + 1]);
        }
        else {
            heading = google.maps.geometry.spherical.computeHeading(this.path[this.svPosition - 1], this.path[this.svPosition]);
        }
        StreetViewFlyby.showStreetView(this.map, this.path[this.svPosition], heading);
        setTimeout(function () {
            if (_this.svRunning) {
                _this.svPosition++;
                _this.streetViewFlybyNext();
            }
        }, 1000);
    };
    StreetViewFlyby.showStreetView = function (map, location, heading) {
        "use strict";
        if (heading === void 0) { heading = 0; }
        var panorama = map.getStreetView();
        panorama.setPosition(location);
        panorama.setVisible(true);
        panorama.setPov({ heading: heading, pitch: 0 });
    };
    return StreetViewFlyby;
}());
function calculateGradient(name, series, pos) {
    "use strict";
    var offset = 0.1;
    var showGradient = series.data[pos][0] > offset;
    if (showGradient) {
        var calculateFrom = pos;
        while ((series.data[pos][0] - series.data[calculateFrom][0] < offset) && (calculateFrom > 0)) {
            calculateFrom--;
        }
        var elevationDiff = series.data[pos][1] - series.data[calculateFrom][1];
        var distance = 1000 * (series.data[pos][0] - series.data[calculateFrom][0]);
        return name + ": " + Math.round(100 * elevationDiff / distance) + "%<br/>";
    }
    return "";
}
function elevationFormatter(v) {
    "use strict";
    return addCommas(roundNumber(v, 1)) + "m";
}
function getClimbTotalAscent(elevations) {
    "use strict";
    var ascent = 0;
    for (var i = 0; i < elevations.length; i++) {
        if (i > 0) {
            var thisAscent = elevations[i] - elevations[i - 1];
            if (thisAscent > 0) {
                ascent += thisAscent;
            }
        }
    }
    return ascent;
}
function getClimbGrade(climbInMetres, segmentDistanceInMetres) {
    "use strict";
    return 100 * climbInMetres / segmentDistanceInMetres;
}
function getClimbScore(climbInMetres, segmentDistanceInMetres) {
    "use strict";
    var grade = getClimbGrade(climbInMetres, segmentDistanceInMetres);
    return segmentDistanceInMetres * grade;
}
function getClimbCategory(climbInMetres, segmentDistanceInMetres) {
    "use strict";
    var grade = getClimbGrade(climbInMetres, segmentDistanceInMetres);
    // calculate the category
    var score = getClimbScore(climbInMetres, segmentDistanceInMetres);
    var climbCategory = "?";
    if (score < 8000) {
        climbCategory = "NC";
    }
    else if (grade < 3) {
        climbCategory = "NC";
    }
    else {
        if (score >= 8000) {
            climbCategory = "4";
        }
        if (score >= 16000) {
            climbCategory = "3";
        }
        if (score >= 32000) {
            climbCategory = "2";
        }
        if (score >= 64000) {
            climbCategory = "1";
        }
        if (score >= 80000) {
            climbCategory = "HC";
        }
    }
    return climbCategory;
}
function getClimbCategoryDescription(climbCategory) {
    "use strict";
    switch (climbCategory) {
        case 0:
            return "NC";
        case 1:
            return "4";
        case 2:
            return "3";
        case 3:
            return "2";
        case 4:
            return "1";
        case 5:
            return "HC";
        default:
            return "NC";
    }
}
function getDisplayName(detail) {
    "use strict";
    if (detail == null || detail.name == null) {
        return "(no name)";
    }
    return detail.name;
}
function polylineColor(detail, isSelected) {
    "use strict";
    if (isSelected) {
        return "Green";
    }
    if (detail.athlete_count < 0) {
        return "Gray";
    }
    return "Red";
}
function polylineWidth(detail) {
    "use strict";
    if (detail.athlete_count < 0) {
        return 2;
    }
    if (detail.athlete_count < 10) {
        return 1;
    }
    return Math.floor(detail.athlete_count.toString().length - 1);
}
function setInfowindowContent(infowindow, detail) {
    "use strict";
    if (detail == null) {
        return;
    }
    var name = getDisplayName(detail);
    var distance = detail.distance;
    var averageGrade = detail.average_grade;
    var elevationDifference = detail.elevation_high - detail.elevation_low;
    var climbCategory = detail.climb_category;
    var id = detail.id;
    var distanceString = showDistance(distance);
    var content = "<div class=\"segment-details\"><h5>" + name + "</h5>" +
        "<div><table class=\"strava-segment-table\">" +
        "<tr><td><b>Distance</b></td><td>" + distanceString + "</td></tr>";
    content += "<tr><td><b>Average grade</b></td><td>" + averageGrade + " %</td></tr>";
    if (detail.maximum_grade !== 0) {
        content += "<tr><td><b>Maximum grade</b></td><td>" + detail.maximum_grade + "%</td></tr>";
    }
    content += "<tr><td><b>Elevation difference</b></td><td>" + showElevation(elevationDifference) + "</td></tr>";
    if (detail.athlete_count > -1) {
        content += "<tr><td><b>Total elevation gain</b></td><td>" +
            showElevation(detail.total_elevation_gain) +
            "</td></tr>";
    }
    if (climbCategory > 0) {
        content += "<tr><td><b>Climb category</b></td><td>" + getClimbCategoryDescription(climbCategory) + "</td></tr>";
    }
    if (detail.kom_time != null) {
        content += "<tr><td><b>KOM time</b></td><td>" + toHHMMSS(detail.kom_time) + "</td></tr>";
    }
    if (detail.athlete_segment_stats != null && detail.athlete_segment_stats.pr_elapsed_time != null) {
        content += "<tr><td><b>Your PR time</b></td><td>" + toHHMMSS(detail.athlete_segment_stats.pr_elapsed_time) +
            " on " + dateToString(detail.athlete_segment_stats.pr_date) + "</td></tr>";
    }
    if (detail.athlete_count > 0) {
        content += "<tr><td><b># Athletes</b></td><td>" + addCommas(detail.athlete_count) + "</td></tr>";
    }
    if (detail.effort_count > 0) {
        content += "<tr><td><b># Tries</b></td><td>" + addCommas(detail.effort_count) + "</td></tr>";
    }
    if (detail.star_count > 0) {
        content += "<tr><td><b># Stars</b></td><td>" + addCommas(detail.star_count);
        if (detail.starred) {
            content += " (including you)";
        }
        content += "</td></tr>";
    }
    content += "</table></div>";
    if (detail.elevation != null) {
        content += "<div class=\"elevation-chart\"></div>";
    }
    content += "<div>" + "<a target=\"_blank\" href=\"StravaSegment.php?id=" + id + "\">Details</a> | " +
        "<a target=\"_blank\" href=\"https://www.strava.com/segments/" + id + "\">View on Strava</a> | " +
        "<form id=\"strava-download-gpx\" action=\"download.ashx\" method=\"post\">" +
        "<input type=\"hidden\" name=\"fileName\" value=\"segment.gpx\" />" +
        "<textarea style=\"display:none;\" id=\"gpxData\" name=\"data\" spellcheck=\"false\"></textarea>" +
        "<a style=\"cursor:pointer;\" onclick=\"downloadGpx(" + id + ")\">GPX</a>" +
        "</form></div></div>";
    infowindow.setContent(content);
    // plot the chart
    if (detail.elevation != null && $(".elevation-chart").length > 0) {
        var minElevation = 100000;
        var maxElevation = -100000;
        var elevData = [];
        for (var i = 0; i < detail.elevation.length; i++) {
            elevData.push([detail.elevation[i].d, detail.elevation[i].a]);
            minElevation = Math.min(minElevation, detail.elevation[i].a);
            maxElevation = Math.max(maxElevation, detail.elevation[i].a);
        }
        var chartMin = elevationChartMin(minElevation, maxElevation);
        var chartMax = elevationChartMax(minElevation, maxElevation);
        $.plot($(".elevation-chart"), [{ data: elevData, lines: { show: true, fill: true, fillColor: "#D9D9D9" } }], {
            grid: {
                aboveData: true
            },
            yaxis: {
                min: chartMin,
                max: chartMax
            },
            colors: ["#D9D9D9"]
        });
    }
}
function starSegment(id) {
    $.ajax({
        url: "StravaStarSegment.ashx?id=" + id,
        success: function () {
            alert("success");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showError(errorThrown);
        }
    });
}
function dateToString(stravaDate) {
    "use strict";
    var monthNames = [
        "January", "February", "March",
        "April", "May", "June", "July",
        "August", "September", "October",
        "November", "December"
    ];
    var date = new Date(stravaDate);
    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    return day + " " + monthNames[monthIndex] + " " + year;
}
function toHHMMSS(sec_num) {
    "use strict";
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);
    var hoursString = hours.toString();
    if (hours < 10) {
        hoursString = "0" + hours;
    }
    var minutesString = minutes.toString();
    if (minutes < 10) {
        minutesString = "0" + minutes;
    }
    var secondsString = seconds.toString();
    if (seconds < 10) {
        secondsString = "0" + seconds;
    }
    return hoursString + ":" + minutesString + ":" + secondsString;
}
function downloadGpx(id) {
    "use strict";
    $.ajax("StravaSegmentDetail.ashx?id=" + id, {
        cache: false,
        success: function (data) {
            // build the GPX
            var gpx = gpxStart(data.name);
            for (var i = 0; i < data.elevation.length; i++) {
                var pt = data.elevation[i];
                var ll = new google.maps.LatLng(pt.l[0], pt.l[1]);
                gpx += gpxPoint(ll, pt.a);
            }
            gpx += gpxEnd();
            $("#gpxData").val(gpx);
            $("#strava-download-gpx").submit();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            showError("Error - " + errorThrown);
        }
    });
}
function getSegmentFromMarker(marker) {
    "use strict";
    if (marker == null) {
        return null;
    }
    return marker.detail;
}
//#endregion 
