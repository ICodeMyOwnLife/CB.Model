using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using Prism.Commands;


namespace CB.Model.Localization
{
    public class LocalCulture: IEquatable<LocalCulture>
    {
        #region Fields
        private static readonly ISet<LocalCulture> _cultures = new HashSet<LocalCulture>();
        #endregion


        #region  Constructors & Destructor
        private LocalCulture(string code, string name)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            Code = code;
            Name = name;
            ApplyCommand = new DelegateCommand(Apply);
        }
        #endregion


        #region  Commands
        public ICommand ApplyCommand { get; }
        #endregion


        #region  Properties & Indexers
        public static IEnumerable<LocalCulture> Cultures => _cultures;
        public static LocalCulture CurrentCulture { get; private set; }
        public string Code { get; }
        public string Name { get; }
        #endregion


        #region Methods
        public static LocalCulture Add(string code, string name)
        {
            var localCulture = new LocalCulture(code, name);
            _cultures.Add(localCulture);
            return localCulture;
        }

        public static LocalCulture AddAndApply(string code, string name)
        {
            var localCulture = Add(code, name);
            localCulture.Apply();
            return localCulture;
        }

        public void Apply()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Code);
            CurrentCulture = this;
        }

        public bool Equals(LocalCulture other)
            => StringComparer.InvariantCultureIgnoreCase.Equals(Code, other.Code);
        #endregion


        #region Override
        public override bool Equals(object obj)
            => obj is LocalCulture && Equals((LocalCulture)obj);

        public override int GetHashCode()
            => Code.GetHashCode();

        public override string ToString()
            => $"{Name} ({Code})";
        #endregion
    }
}