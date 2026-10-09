
namespace Mocha;

public static class Icons
{
    public const string UNKNOWN = "question_mark";

    public const string HOME = "home";
    public const string BEDTIME = "bedtime";
    public const string SLEEP = "hotel";
    public const string SUNRISE = "backlight_high";
    public const string WEATHER = "cloud";
    public const string NAV_BAR_CLOSE = "menu_open";
    public const string NAV_BAR_OPEN = "menu";
    public const string ADD = "add";
    public const string PERSON = "person";
    public const string LOGIN = "login";
    public const string JOURNAL = "collections_bookmark";
    public const string GRAPH = "bar_chart";
    public const string EXPORT = "upload";
    public const string SETTINGS = "settings";
    public const string DELETE = "delete";
    public const string EDIT = "edit";
    public const string CLOSE = "close";
    public const string RATING = "kid_star";
    public const string PRODUCTIVITY_RATING = "checklist";
    public const string MOOD_RATING = "psychiatry";
    public const string HEALTH_STEPS = "steps";
    public const string HEALTH_HEART_PTS_GOOD = "heart_check";
    public const string HEALTH_HEART_PTS_BAD = "heart_minus";
    public const string HEALTH_MAP = "map";
    public const string HIKING = "hiking";
    public const string HEALTH_CALORIES = "mode_heat";
    public const string BACK = "arrow_back";
    public const string CALENDAR = "calendar_today";
    public const string CALENDAR_CHECK = "calendar_check";
    public const string CALENDAR_DUE = "priority_high";
    public const string ARROW_BACK = "arrow_back";
    public const string ARROW_FORWARD = "arrow_forward";
    public const string HEART_RATE = "ecg_heart";
    public const string SPO2 = "spo2";
    public const string LIGHT_MODE = "light_mode";
    public const string DARK_MODE = "dark_mode";
    public const string GO_BACK = "heart_broken";
    public const string DESKTOP_WINDOW = "desktop_windows";
    public const string LOGOUT = "logout";
    public const string TASKS = "assignment";
    public const string CHECK = "check";
    public const string PLAY = "play_arrow";
    public const string DOTS = "more_horiz";
    public const string REPEAT = "repeat";
    public const string ENERGY = "bolt";
    public const string REMOVE = "remove";
    public const string UNDO = "undo";
    public const string VISIBILITY = "visibility";
    public const string VISIBILITY_OFF = "visibility_off";
    public const string PROJECTS = "folder";


    public static string GetHeartPtsIcon(int heartPts)
    {
        return heartPts >= 20 ? HEALTH_HEART_PTS_GOOD : HEALTH_HEART_PTS_BAD;
    }

    // public static string GetForWeatherType(Weather weather)
    // {
    //     return weather switch
    //     {
    //         Weather.Cloudy => "cloud",
    //         Weather.Unknown => UNKNOWN,
    //         Weather.Sunny => "sunny",
    //         Weather.PartlyCloudy => "partly_cloudy_day",
    //         Weather.Rainy => "rainy",
    //         Weather.Snowy => "weather_snowy",
    //         Weather.Sleet or Weather.Hail => "weather_hail",
    //         Weather.Stormy => "thunderstorm",
    //         Weather.Windy => "air",
    //         Weather.Foggy => "foggy",
    //         _ => UNKNOWN
    //     };
    // }
}

