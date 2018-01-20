//#region elevation calculation
var ElevationCalculator = (function () {
    function ElevationCalculator(locations, callback, useGoogle, completeCallback) {
        this.elevations = [];
        this.stop = false;
        this.locations = locations;
        this.callback = callback;
        this.useGoogle = useGoogle;
        this.completeCallback = completeCallback;
    }
    ElevationCalculator.prototype.calculate = function () {
        if (this.locations.length === 0) {
            this.callback("Elevation calculation failed. No locations were supplied");
            return;
        }
        this.callback("Calculating elevation for " + this.locations.length + " locations...");
        this.elevations = [];
        this.currentPos = 0;
        this.getElevation();
    };
    ElevationCalculator.prototype.stopCalculation = function () {
        this.stop = true;
    };
    ElevationCalculator.prototype.getElevation = function () {
        "use strict";
        if (this.stop) {
            this.callback("Stopping calculation");
            return;
        }
        // calculate the elevation of the route
        var locationsPart = [];
        var endPos = Math.min(this.locations.length, this.currentPos + 100);
        for (var i = this.currentPos; i < endPos; i++) {
            locationsPart.push(this.locations[i]);
            //console.log(this.locations[i]);
        }
        this.callback("Calculating elevation for " + this.currentPos + " to " + endPos + " (of " + this.locations.length + ")...");
        if (this.useGoogle) {
            this.calculateElevationWithGoogle(locationsPart);
        }
        else {
            this.calculateElevationWithBing(locationsPart);
        }
    };
    ElevationCalculator.prototype.calculateElevationWithGoogle = function (locationsPart) {
        "use strict";
        var _this = this;
        var positionalRequest = {
            locations: locationsPart
        };
        var elevator = new google.maps.ElevationService();
        // initiate the location request
        elevator.getElevationForLocations(positionalRequest, function (results, status) {
            if (status === google.maps.ElevationStatus.OK) {
                for (var i = 0; i < results.length; i++) {
                    _this.elevations.push(results[i].elevation);
                }
                _this.moveNextOrFinish();
            }
            else {
                if (status === google.maps.ElevationStatus.OVER_QUERY_LIMIT) {
                    var end = Math.min(_this.currentPos + 100, _this.locations.length);
                    _this.callback("Over query limit calculating the elevation for " + _this.currentPos + " to " +
                        end + " (of " + _this.locations.length + "), waiting 1 second before retrying");
                    setTimeout(function () { _this.getElevation(); }, 1000);
                }
                else {
                    _this.callback("An error occurred calculating the elevation - " +
                        ElevationCalculator.elevationStatusDescription(status));
                    _this.completeCallback(null);
                }
            }
        });
    };
    ElevationCalculator.elevationStatusDescription = function (status) {
        "use strict";
        switch (status) {
            case google.maps.ElevationStatus.OVER_QUERY_LIMIT:
                return "Over query limit";
            case google.maps.ElevationStatus.UNKNOWN_ERROR:
                return "Unknown error";
            case google.maps.ElevationStatus.INVALID_REQUEST:
                return "Invalid request";
            case google.maps.ElevationStatus.REQUEST_DENIED:
                return "Request denied";
            default:
                return status.toString();
        }
    };
    ElevationCalculator.prototype.calculateElevationWithBing = function (locationsPart) {
        "use strict";
        var _this = this;
        // make an AJAX request to Bing
        var url = "https://dev.virtualearth.net/REST/v1/Elevation/List?" +
            "key=AntrTMH-ZJdIfOHs2kyTIcG333TAMGGGU6LcvAd4glga_5ekMcKENnJ1AWf8jrwB";
        url += "&points=" + this.encodePoints(locationsPart);
        $.ajax(url, {
            dataType: "jsonp",
            jsonp: "jsonp",
            success: function (data) {
                // read the data
                var results = data.resourceSets[0].resources[0].elevations;
                for (var i = 0; i < results.length; i++) {
                    _this.elevations.push(results[i]);
                }
                _this.moveNextOrFinish();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                _this.callback("An error occurred calculating the elevation - " + errorThrown);
                _this.completeCallback(null);
            }
        });
    };
    ElevationCalculator.prototype.encodePoints = function (points) {
        "use strict";
        var latitude = 0;
        var longitude = 0;
        var result = [];
        for (var i = 0; i < points.length; i++) {
            var point = points[i];
            // step 2
            var newLatitude = Math.round(point.lat() * 100000);
            var newLongitude = Math.round(point.lng() * 100000);
            // step 3
            var dy = newLatitude - latitude;
            var dx = newLongitude - longitude;
            latitude = newLatitude;
            longitude = newLongitude;
            // step 4 and 5
            dy = (dy << 1) ^ (dy >> 31);
            dx = (dx << 1) ^ (dx >> 31);
            // step 6
            var index = ((dy + dx) * (dy + dx + 1) / 2) + dy;
            while (index > 0) {
                // step 7
                var rem = index & 31;
                index = (index - rem) / 32;
                // step 8
                if (index > 0) {
                    rem += 32;
                }
                // step 9
                result.push("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-"[rem]);
            }
        }
        // step 10
        return result.join("");
    };
    ElevationCalculator.prototype.moveNextOrFinish = function () {
        "use strict";
        if (this.stop) {
            this.callback("Stopping calculation");
            return;
        }
        this.currentPos += 100;
        if (this.currentPos >= this.locations.length) {
            this.callback("Elevation calculated using " + this.locations.length + " locations");
            this.completeCallback(this.elevations);
        }
        else {
            this.getElevation();
        }
    };
    return ElevationCalculator;
}());
//#endregion 
