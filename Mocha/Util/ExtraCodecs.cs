// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using DotNetty.Buffers;
using Nocturne.Database.API;

namespace Mocha.Util;

public static class ExtraCodecs
{
    public static readonly Codec<DateTimeOffset> DATE_TIME_OFFSET = Codecs.LONG.Transform(DateTimeOffset.FromUnixTimeMilliseconds, dto => dto.ToUnixTimeMilliseconds());
    public static readonly IBinaryCodec<DateTimeOffset> DATE_TIME_OFFSET_BINARY = BinaryCodecs.LONG.Transform(dto => dto.ToUnixTimeMilliseconds(), DateTimeOffset.FromUnixTimeMilliseconds);
    public static readonly IBinaryCodec<TimeSpan> TIME_SPAN_BINARY = BinaryCodecs.LONG.Transform(span => (long)span.TotalMilliseconds, TimeSpan.FromMilliseconds);

    public static readonly IBinaryCodec<DateOnly> DATE_ONLY_BINARY = BinaryCodecs.INT.Transform(d => d.DayNumber, DateOnly.FromDayNumber);

    public static readonly NocturneKeySerializer<DateOnly> DATE_ONLY_KEY_SERIALIZER = new InlineKeySerializer<DateOnly>(
        (buffer, value) => buffer.WriteInt(value.DayNumber),
        buffer => DateOnly.FromDayNumber(buffer.ReadInt()),
        (left, right) => left.CompareTo(right)
    );

    internal class InlineKeySerializer<T>(Action<IByteBuffer, T> read, Func<IByteBuffer, T> write, Func<T, T, int> compare) : NocturneKeySerializer<T>
    {
        public override T Read(IByteBuffer buffer) => write.Invoke(buffer);

        public override void Write(IByteBuffer buffer, T value) => read.Invoke(buffer, value);

        protected override int CompareValues(T left, T right) => compare.Invoke(left, right);
    }
}
