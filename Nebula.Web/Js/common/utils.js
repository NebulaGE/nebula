
    function Utils() {
        var self = this;
        self.getLetterByIndex = function(index) {
            switch(index) {
                case 0:
                    return 'ა';             
                case 1:
                    return 'ბ';
                
                case 2:
                    return 'გ';
                
                case 3:
                    return 'დ';
                   
                case 4:
                    return 'ე';
                   
                case 5:
                    return 'ვ';
                  
                case 6:
                    return 'ზ';
                    
            }
            return null;
        }

        self.timeFormat = function (time, word) {

            if (time.toString().length < 2) {
                return '<span class="timer-in"> 0' + time + '   <span class="time-word">' + word + '</span></span>';
            }
            return '<span class="timer-in"> ' + time + '   <span class="time-word">' + word + '</span></span>';
        }

        self.arrayFirstIndexOf =   function (array, predicate, predicateOwner) {
            for (var i = 0, j = array.length; i < j; i++) {
                if (predicate.call(predicateOwner, array[i])) {
                    return i;
                }
            }
            return -1;
        }

        self.ckUpdate = function () {
        
                for (instance in CKEDITOR.instances)
                    CKEDITOR.instances[instance].updateElement();
            
        }
    }

    window.utils = new Utils();
