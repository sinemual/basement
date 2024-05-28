using System;
using System.Collections;
using System.Globalization;
using Client;
using Client.Data.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class TimeManagerService
{
    private SharedData _data;
    private ICoroutineRunner _coroutineRunner;
    
    private DateTimeOffset _netTime;
    public DateTimeOffset NetTime => _netTime;
    
    public UnityAction OnSecondUpdate;
    
    public TimeManagerService(SharedData data, ICoroutineRunner coroutineRunner)
    {
        _data = data;
        _coroutineRunner = coroutineRunner;
        _coroutineRunner.StartCoroutine(GetNetTime());
        _coroutineRunner.StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        yield return new WaitForSeconds(1.0f);
        OnSecondUpdate.Invoke();
        if (_netTime.ToString() != "")
            _netTime = NetTime.AddSeconds(Time.deltaTime);
    }

    private IEnumerator GetNetTime()
    {
        var myHttpWebRequest = UnityWebRequest.Get("https://www.google.com");
        yield return myHttpWebRequest.SendWebRequest();

        if (myHttpWebRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("NETWORK ERROR");
            //_netTime = null;
            yield break;
        }

        var netTimeString = myHttpWebRequest.GetResponseHeader("date");
        if (netTimeString != "")
            _netTime = DateTimeOffset.ParseExact(netTimeString,
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                CultureInfo.InvariantCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal);
        
        //Debug.Log("Global UTC time: " + netTime);
    }

    public void SaveWithNetTime(ref string key, TimeSpan add) => key = ToStrCurrentTime(add);

    public void AddTimeToExistTimer(ref string key, TimeSpan add) => SaveWithSpecifiedValue(ref key, LoadDateTimeOffset(ref key).Add(add));

    public bool IsNetTimeMoreThanSavedValue(ref string key)
    {
        var saved = LoadDateTimeOffset(ref key);
        return (NetTime - saved).TotalSeconds >= 0.0f;
    }

    public TimeSpan GetTime(ref string key)
    {
        var diff = GetTimeDifference(ref key);
        if (diff.TotalSeconds < 0f)
            return new TimeSpan(0, 0, 0);
        return diff;
    }

    public DateTimeOffset LoadDateTimeOffset(ref string key)
    {
        if (!string.IsNullOrEmpty(key))
            return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(key));
        return DateTimeOffset.Now;
    }

    #region Dont use - it's internal (Low level api)

    private TimeSpan GetTimeDifference(ref string key, bool isCurrentMore = false)
    {
        if (isCurrentMore)
        {
            var dtoDiff = ToDTONetTimeAsUnixTime() - LoadDateTimeOffset(ref key);
            return new TimeSpan(dtoDiff.Hours, dtoDiff.Minutes, dtoDiff.Seconds);
        }
        else
        {
            var dtoDiff = LoadDateTimeOffset(ref key) - ToDTONetTimeAsUnixTime();
            return new TimeSpan(dtoDiff.Hours, dtoDiff.Minutes, dtoDiff.Seconds);
        }
    }

    private void SaveWithSpecifiedValue(ref string key, DateTimeOffset dto)
    {
        key = dto.ToUnixTimeMilliseconds().ToString();
    }

    private string ToStrCurrentTime(TimeSpan add)
    {
        return NetTime.Add(add).ToUnixTimeMilliseconds().ToString();
    }

    private DateTimeOffset ToDTOCurrentTimeAsUnixTime()
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(
            long.Parse(DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()));
    }

    private DateTimeOffset ToDTONetTimeAsUnixTime()
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(NetTime.ToUnixTimeMilliseconds().ToString()));
    }

    #endregion
}