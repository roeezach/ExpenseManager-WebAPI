export const fileTypeParser = (fileType:string)=> {
    if(fileType === 'Habinleumi')
        return 0;
    else if(fileType === 'Hapoalim')
        return 1;
    else if(fileType === 'Max')
        return 2;
    else
        return -1;
  }
