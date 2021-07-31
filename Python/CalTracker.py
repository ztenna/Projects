'''
Created on May 26, 2021

@author: Zachary Tennant
'''
from tkinter import *
from datetime import datetime
import plotly.graph_objects as go

class CalTracker:

    # GET TODAY INFO
    def getTodayInfo(self, location):
        totalCals = 0.0
        items = []
        cals = []
        
        if (location == "Ingestion"):
            items.append("Items")
        elif (location == "Exercise"):
            items.append("Exercise")
        cals.append("Calories")
        
        try:
            if (location == "Ingestion"):
                file = open(datetime.today().strftime('%m-%d-%Y') + " Ingested Cal Track.txt", "r")
            elif (location == "Exercise"):
                file = open(datetime.today().strftime('%m-%d-%Y') + " Burned Cal Track.txt", "r")
            
            for line in file:
                tempSplitStr = line.split()
                item = ""
                cal = 0.0
                for word in tempSplitStr:
                    try:
                        tempCal = float(word)
                        if (isinstance(tempCal, float)):
                            cal = tempCal
                    except ValueError:
                        item += word
                        item += " "
                items.append(item)
                cals.append(cal)
                totalCals += cal
                
            file.close()
            return items, cals, totalCals
                
        except FileNotFoundError:
            return items, cals, 0.0
    print()
        
    #=================================================================================================#
    
    # GET LAST USED DATE AND MONTHLY TOTALS
    def getMonthlyTotals(self):
        try:
            monthlyFile = open("monthlyTotals.txt", "r")
            self.lastTimeUsed = monthlyFile.readline().strip()
            self.ingestionMonthlyTotal = float(monthlyFile.readline())
            self.exerciseMonthlyTotal = float(monthlyFile.readline())
            monthlyFile.close() 
        except FileNotFoundError:
            self.ingestionMonthlyTotal = 0.0
            self.exerciseMonthlyTotal = 0.0
            self.lastTimeUsed = datetime.today().strftime('%m %Y')
    print()

    #=================================================================================================#
    
    # SAVE LAST USED DATE AND MONTHLY TOTALS WHEN RED X IS CLICKED
    def saveMonthlyTotals(self, window):
        monthlyFile = open("monthlyTotals.txt", "w")
        monthlyFile.write(datetime.today().strftime('%m %Y') + '\n')
        monthlyFile.write(str(self.ingestionMonthlyTotal) + '\n')
        monthlyFile.write(str(self.exerciseMonthlyTotal))
        monthlyFile.close()
        window.destroy()
    print()
    
    #=================================================================================================#

    # SET LOCATION OF ENTITIES
    def setLocation(self, entity):
        entity.update_idletasks()
    
        entityHeight = entity.winfo_height()
        entityWidth = entity.winfo_width()
        
        screenHeight = entity.winfo_screenheight()
        screenWidth = entity.winfo_screenwidth()
        
        xCor = int((screenWidth/2) - (entityWidth/2))
        yCor = int((screenHeight/2) - (entityHeight/2))
        
        entity.geometry("{}x{}+{}+{}".format(entityWidth, entityHeight, xCor, yCor))
    print()
    
    #=================================================================================================#

    # DISPLAY ERROR MESSAGE
    def showErrorMessage(self, msg):
        errorMessageFrame = Tk()
        errorMessageFrame.title("RED ALERT")
        
        msgLabel = Label(errorMessageFrame,
                         text = msg,
                         width = 55,
                         fg = "white",
                         bg = "red",
                         borderwidth = 1,
                         relief = "solid")
        msgLabel.pack(side = TOP)
        
        self.setLocation(errorMessageFrame)
    print()
    
    #=================================================================================================#

    # VIEW TABLE
    def viewTable(self, location, items, itemCals, totalCals, window):
        
        def addButtonClick(table):
            
            def addEntityButtonClick(table, addToListFrame):
                cals = float(calEntry.get())
                if (isinstance(cals, float)):
                    if (location == "Ingestion"):
                        self.todayItems.append(sourceEntry.get())
                        self.todayIngCals.append(cals)
                        self.todayTotalIngCals += float(cals)
                        self.ingestionMonthlyTotal += float(cals)
                        
                        items = self.todayItems
                        itemCals = self.todayIngCals
                        totalCals = self.todayTotalIngCals
                    elif (location == "Exercise"):
                        self.todayEx.append(sourceEntry.get())
                        self.todayExCals.append(cals)
                        self.todayTotalExCals += float(cals)
                        self.exerciseMonthlyTotal += float(cals)
                        
                        items = self.todayEx
                        itemCals = self.todayExCals
                        totalCals = self.todayTotalExCals
                        
                    table.destroy()
                    addToListFrame.destroy()
                    self.viewTable(location, items, itemCals, totalCals, window)
            print()
            
            #-----------------------------------------------------------------------------------------#
            
            addToListFrame = Tk()
            addToListFrame.title("Add to " + location)
            
            addFrame = Frame(addToListFrame)
            addFrame.pack(side = TOP)
            
            sourceFrame = Frame(addFrame)
            sourceFrame.pack(side = TOP)
            
            calFrame = Frame(addFrame)
            calFrame.pack(side = BOTTOM)
            
            addButtonFrame = Frame(addToListFrame)
            addButtonFrame.pack(side = BOTTOM)
            
            if (location == "Ingestion"):
                sourceLabel = Label(sourceFrame,
                                    text = "Item:",
                                    width = 5,
                                    height = 1)
            elif (location == "Exercise"):
                sourceLabel = Label(sourceFrame,
                                    text = "Exercise:",
                                    width = 9,
                                    height = 1)
            sourceLabel.pack(side = LEFT)
            
            sourceEntry = Entry(sourceFrame,
                                width = 20,
                                fg = "black",
                                bg = "white")
            sourceEntry.pack(side = LEFT)
            
            calLabel = Label(calFrame,
                             text = "Calories:",
                             width = 9,
                             height = 1)
            calLabel.pack(side = LEFT)
            
            calEntry = Entry(calFrame,
                             width = 6,
                             fg = "black",
                             bg = "white")
            calEntry.pack(side = LEFT)
            
            addButton = Button(addButtonFrame,
                               text = "Add",
                               fg = "white",
                               bg = "blue",
                               width = 3,
                               height = 1,
                               command = lambda: addEntityButtonClick(table, addToListFrame))
            addButton.pack(side = LEFT)
            
            self.setLocation(addToListFrame)
        print()
            
        #---------------------------------------------------------------------------------------------#
        
        # SEE IF USER WANTS TO REMOVE ROW FROM TABLE
        def removeRow(location, index, cals):
            questionFrame = Tk()
            questionFrame.title("To Remove Or Not To Remove")
            
            question1 = "Do you want to remove "
            question2 = "?"
            
            questionLabel = Label(questionFrame,
                             text = question1 + items[index] + question2,
                             fg = "black",
                             bg = "white",
                             borderwidth = 1,
                             relief = "solid")
            questionLabel.pack(side = TOP)
            
            removeButtonFrame = Frame(questionFrame)
            removeButtonFrame.pack(side = BOTTOM)
            
            def yesButtonClick():
                if (location == "Ingestion"):
                    self.todayItems.pop(index)
                    self.todayIngCals.pop(index)
                    self.todayTotalIngCals -= cals
                    self.ingestionMonthlyTotal -= cals
                    
                    items = self.todayItems
                    itemCals = self.todayIngCals
                    totalCals = self.todayTotalIngCals
                elif (location == "Exercise"):
                    self.todayEx.pop(index)
                    self.todayExCals.pop(index)
                    self.todayTotalExCals -= cals
                    self.exerciseMonthlyTotal -= cals
                    
                    items = self.todayEx
                    itemCals = self.todayExCals
                    totalCals = self.todayTotalExCals
                    
                questionFrame.destroy()
                table.destroy()
                self.viewTable(location, items, itemCals, totalCals, window)
            
            def noButtonClick():
                questionFrame.destroy()
            
            yesButton = Button(removeButtonFrame,
                               text = "YES",
                               fg = "white",
                               bg = "blue",
                               width = 3,
                               height = 1,
                               command = yesButtonClick)
            yesButton.pack(side = LEFT)
            
            noButton = Button(removeButtonFrame,
                              text = "NO",
                              fg = "white",
                              bg = "blue",
                              width = 2,
                              height = 1,
                              command = noButtonClick)
            noButton.pack(side = LEFT)
        print()    
        
        #---------------------------------------------------------------------------------------------#    
            
        # REMOVE ROW FROM TABLE
        def removeFromList(event):
            gridInfo = event.widget.grid_info()
            index = gridInfo["row"]
            
            cals = itemCals[index]
                
            if (isinstance(cals, float)):
                removeRow(location, index, cals)
            else:
                self.showErrorMessage("Can't Remove This Row")   
        print()
            
        #---------------------------------------------------------------------------------------------#
        
        # SAVE DAILY INFO WHEN RED X IS CLICKED
        def saveDailyInfo():
            if (location == "Ingestion"):
                file = open(datetime.today().strftime('%m-%d-%Y') + " Ingested Cal Track.txt", "w")
            elif (location == "Exercise"):
                file = open(datetime.today().strftime('%m-%d-%Y') + " Burned Cal Track.txt", "w")
            
            for i in range(len(items) - 1):
                file.write(items[i + 1] + " " + str(itemCals[i + 1]) + '\n')
            
            file.close()
            table.destroy()
            window.destroy()
            self.createMainTable()
        print()
        
        #---------------------------------------------------------------------------------------------#
            
        table = Tk()
        table.title(location)
        table.protocol("WM_DELETE_WINDOW", saveDailyInfo)
        
        todayTotalFrame = Frame(table)
        todayTotalFrame.pack(side = TOP)
        
        totalLabel = Label(todayTotalFrame,
                           text = "Total Calories:",
                           width = 14,
                           height = 1)
        totalLabel.pack(side = LEFT)
        
        totalEntry = Label(todayTotalFrame,
                           text = totalCals,
                           width = 10,
                           fg = "black",
                           bg = "white",
                           borderwidth = 1,
                           relief = "solid")
        totalEntry.pack(side = LEFT)
        
        tableFrame = Frame(table)
        tableFrame.pack(side = TOP)
        
        entries = []
        
        for i in range(len(items)):
            row = []
            row.append(items[i])
            row.append(itemCals[i])
            entries.append(row)
    
        for i in range(len(items)):
            for j in range(2):
                cell = Label(tableFrame,
                             text = entries[i][j], 
                             width = 20, 
                             fg = "black",
                             bg = "white",
                             borderwidth = 1,
                             relief = "solid")
                cell.grid(row = i, column = j)
                cell.bind("<Button-3>", removeFromList)
                
        tableButtonFrame = Frame(table)
        tableButtonFrame.pack(side = BOTTOM)
        
        addButton = Button(tableButtonFrame,
                           text = "Add",
                           fg = "white",
                           bg = "blue",
                           width = 3,
                           height = 1,
                           command = lambda: addButtonClick(table))
        addButton.pack(side = LEFT)
                
        self.setLocation(table)
    print()
        
    #=================================================================================================#
    
    # VIEW MONTHLY TABLE
    def viewMonthlyTable(self):
        dates = []
        ingCals = []
        exCals = []
        
        try:
            listFile = open("ListOfMonthlyTotals.txt", "r")
            for line in listFile:
                entry = line.split(";")
                dates.append(entry[0])
                ingCals.append(entry[1])
                exCals.append(entry[2])
     
            fig = go.Figure()
            fig.add_trace(go.Scatter(x = dates, y = ingCals, mode = "lines+markers", name = "Ingestion"))
            fig.add_trace(go.Scatter(x = dates, y = exCals, mode = "lines+markers", name = "Exercise"))
            fig.update_layout(title = "Monthly Totals", xaxis_title = "Month Year", yaxis_title = "Calories")
            
            fig.show()
        except FileNotFoundError:
            self.showErrorMessage("No Monthly Totals Exist. Will be available on the first of next month.")
    print()
        
    #=================================================================================================#
    
    # SAVE MONTHLY TOTALS TO LIFETIME LIST
    def saveTotalsToList(self):
        listFile = open("ListOfMonthlyTotals.txt", "a")
        listFile.write(self.lastTimeUsed + ";" + str(self.ingestionMonthlyTotal) + ";" + str(self.exerciseMonthlyTotal))
        listFile.close()
    print()
    
    #=================================================================================================#
    
    def createMainTable(self):        
        window = Tk()
        window.title("Calorie Tracker: " + datetime.today().strftime('%b %Y'))
        window.protocol("WM_DELETE_WINDOW", lambda: self.saveMonthlyTotals(window))
        
        totalsFrame = Frame(window)
        totalsFrame.pack(side = TOP)
            
        # INGESTION
        ingestionFrame = Frame(totalsFrame)
        ingestionFrame.pack(side = LEFT)
        
        ingestionTotalLabel = Label(ingestionFrame,
                                    text = "Ingestion Total:", 
                                    width = 16,
                                    height = 1)
        ingestionTotalLabel.pack(side = LEFT)
        
        ingestionTotalEntry = Label(ingestionFrame,
                                    text = self.ingestionMonthlyTotal,
                                    width = 10,
                                    fg = "black",
                                    bg = "white",
                                    borderwidth = 1,
                                    relief = "solid")
        ingestionTotalEntry.pack(side = LEFT)
        
        # EXERCISE
        exerciseFrame = Frame(totalsFrame)
        exerciseFrame.pack(side = RIGHT)
        
        exerciseTotalLabel = Label(exerciseFrame,
                                   text = "Exercise Total:", 
                                   width = 15,
                                   height = 1)
        exerciseTotalLabel.pack(side = LEFT)
        
        exerciseTotalEntry = Label(exerciseFrame,
                                   text = self.exerciseMonthlyTotal,
                                   width = 10,
                                   fg = "black",
                                   bg = "white",
                                   borderwidth = 1,
                                   relief = "solid")
        exerciseTotalEntry.pack(side = LEFT)
        
        # BUTTONS    
        def viewButtonClick(location, items, itemCals, totalCals):
            self.viewTable(location, items, itemCals, totalCals, window)
            
        def monthlyTotalsButtonClick():
            self.viewMonthlyTable()
        
        buttonFrame = Frame(window)
        buttonFrame.pack(side = BOTTOM)
        
        ingestionButton = Button(buttonFrame,
                                 text = "View Today's Added Calories",
                                 fg = "white",
                                 bg = "blue",
                                 width = 27,
                                 height = 1,
                                 command = lambda: viewButtonClick("Ingestion", self.todayItems, self.todayIngCals, self.todayTotalIngCals))
        ingestionButton.pack(side = LEFT)
        
        exerciseButton = Button(buttonFrame,
                                text = "View Today's Burned Calories",
                                fg = "white",
                                bg = "blue",
                                width = 28,
                                height = 1,
                                command = lambda: viewButtonClick("Exercise", self.todayEx, self.todayExCals, self.todayTotalExCals))
        exerciseButton.pack(side = LEFT)
        
        viewMonthlyTotalsButton = Button(buttonFrame,
                                         text = "View Totals By Month",
                                         fg = "white",
                                         bg = "blue",
                                         width = 20,
                                         height = 1,
                                         command = monthlyTotalsButtonClick)
        viewMonthlyTotalsButton.pack(side = LEFT)
        
        self.setLocation(window)
        
        window.mainloop()
    print()
    
    #=================================================================================================#

tracker = CalTracker()
tracker.todayItems, tracker.todayIngCals, tracker.todayTotalIngCals = tracker.getTodayInfo("Ingestion")
tracker.todayEx, tracker.todayExCals, tracker.todayTotalExCals = tracker.getTodayInfo("Exercise")
tracker.getMonthlyTotals()
today = str(datetime.today().strftime('%m %Y'))
if (tracker.lastTimeUsed != today):
    tracker.saveTotalsToList()
    tracker.ingestionMonthlyTotal = 0.0
    tracker.exerciseMonthlyTotal = 0.0
tracker.createMainTable()