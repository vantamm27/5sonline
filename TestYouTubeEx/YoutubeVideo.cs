using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestYouTubeEx
{
    [Table(Name="YoutubeVideo")]
    public class YoutubeVideo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public  YoutubeVideo()
        {

            this.Id = "";
            this.Title = "";
            this.PubDate = new DateTime();
            this.YoutubeLink = null;
            this.VideoLink = null;
            this.Thumbnail = null;
            this.Description = "";
            this.Episodes = 0;
            
          
        }
        private int _episodes;
        [Column]
        public int  Episodes
        {
            get {return _episodes;}
            set
            {
                if (value != null)
                {
                    NotifyPropertyChanging("Episodes");
                    _episodes = value;
                    NotifyPropertyChanged("Episodes");
                }
         
            }

        }

        private string _id;
        [Column(IsPrimaryKey = true)]
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("Id");
                    _id = value;
                    NotifyPropertyChanged("Id");

                }

            }
        }
        //private string _key;
        //[Column(IsPrimaryKey = true)]
        //public string Key 
        //{
        //    get { return _key; }
        //    set
        //    {
        //        if (_key != value)
        //        {
        //            NotifyPropertyChanging("Key");
        //            _key = value;
        //            NotifyPropertyChanged("Key");
        //        }
        //    }
        
        //}
        private string _title;
        [Column]
        public string Title 
        {
            get { return _title; }

            set
            {
                if (_title != value)
                {
                    NotifyPropertyChanging("Title");
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        
        public DateTime _pubDate;
        [Column]
        public DateTime PubDate 
        {
            get 
            {
                return _pubDate;
            }
            set
            {
            if (_pubDate != value)
            {
                NotifyPropertyChanging("PubDate");
                _pubDate = value;
                NotifyPropertyChanged("PubDate");
            }
        } }
        private string _youtubeLink;
        [Column]
        public string YoutubeLink
        {
            get { return _youtubeLink;}
            set
            {
            if (_youtubeLink != value)
            {
                NotifyPropertyChanging("YoutubeLink");
                _youtubeLink = value;
                NotifyPropertyChanged("YoutubeLink");
            }
        } }
        private string _videoLink;
        [Column]
        public string VideoLink
        {
            get { return _videoLink;}
            set
            {
                if (_videoLink != value)
                {
                    NotifyPropertyChanging("VideoLink");
                    _videoLink = value;
                    NotifyPropertyChanged("VideoLink");
                }
            }
        }
        private string _thumbnail;
        [Column]
        public string Thumbnail
        {
            get { return _thumbnail;}
            set
            {
                if (_thumbnail != value)
                {
                    NotifyPropertyChanging("Thumbnail");
                    _thumbnail = value;
                    NotifyPropertyChanged("Thumbnail");
                }
            }
        }
        private string _description;
        [Column]
        public string Description
        {
            get { return _description;}
            set
            {
                if (_description != value)
                {
                    NotifyPropertyChanging("Description");
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

       
        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(string propertyName)
        {
            if(PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class YouTubeDataBaseContext:DataContext
    {
        public static string DBConectString = "Data Source=isostore:DataBases.sdf";
        
        public YouTubeDataBaseContext(string connectString): base(connectString)
        {
            this.YoutubeVideos = this.GetTable<YoutubeVideo>();
        }

        public Table<YoutubeVideo> YoutubeVideos;      
           


    }

    public class YouTubeViewModel:INotifyPropertyChanged
    {
        private YouTubeDataBaseContext context = new YouTubeDataBaseContext(YouTubeDataBaseContext.DBConectString);


        public YouTubeViewModel()
        {
            this.YouTubeVideo = new YoutubeVideo();

        }

        public YouTubeViewModel(YoutubeVideo YouTubeVideo)
        {
            this.YouTubeVideo = context.YoutubeVideos.Where(b => b.Id == YouTubeVideo.Id).FirstOrDefault();
        }


        public YoutubeVideo YouTubeVideo
        {
            get { return YouTubeVideo; }
            set
            {
                
                YouTubeVideo.Id = value.Id;
                YouTubeVideo.Description = value.Description;
                NotifyPropertyChanged("YouTubeVideo");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void Save()
        {
            if(YouTubeVideo.Id.Length != 0)
            {
                context.YoutubeVideos.InsertOnSubmit(YouTubeVideo);
            }
            context.SubmitChanges();
        }
               

    }

    public class MainViewModel :INotifyPropertyChanged
    {
        private YouTubeDataBaseContext context;

   //     public ObservableCollection<YoutubeVideo> YouTubeVideos { get; private set; }

        public bool IsDataLoaded { get; private set; }

        public MainViewModel()
        {
            //this.YouTubeVideos = new ObservableCollection<YoutubeVideo>();
            context = new YouTubeDataBaseContext(YouTubeDataBaseContext.DBConectString);
            if(!context.DatabaseExists())
            {
                context.CreateDatabase();
                context.SubmitChanges();
            }
        }

        public int  Count()
        {
            int count = context.YoutubeVideos.Count();
            
             //IsDataLoaded = true;
             return count;
        }

        public bool Save(YoutubeVideo youtubevideo)
        {
            if(context.YoutubeVideos.Where(b=>b.Id == youtubevideo.Id).Count() == 0)
            {
                context.YoutubeVideos.InsertOnSubmit(youtubevideo);
                context.SubmitChanges();
                return true;

            }
            return false;
        }

        public List<YoutubeVideo> Load(int ifrom, int nResult)
        {
            
            List<YoutubeVideo> youtubevideList = context.YoutubeVideos.ToList();
            if(ifrom ==0)
                //return context.YoutubeVideos.OrderBy(x=>x.Title).OrderByDescending(x=> x.PubDate).Take(nResult).ToList<YoutubeVideo>();
                return context.YoutubeVideos.OrderByDescending(x => x.Episodes).Take(nResult).ToList<YoutubeVideo>();

            //return context.YoutubeVideos.OrderByDescending(x => x.PubDate).Skip(ifrom - 1).Take(nResult).ToList<YoutubeVideo>();
            return context.YoutubeVideos.OrderByDescending(x => x.Episodes).Skip(ifrom - 1).Take(nResult).ToList<YoutubeVideo>();
                       
                //       SELECT ROW_NUMBER() 
                //        OVER (ORDER BY EmployeeName) AS Row, 
                //        EmployeeId, EmployeeName, Salary 
                //FROM Employees;


           // YouTubeVideos = new ObservableCollection<YoutubeVideo>(youtubevideList);
           // return youtubevideList;            
        }
      
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
